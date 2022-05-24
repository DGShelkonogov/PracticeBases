using System.Collections;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_bases.Models;
using Practice_bases.ViewModel;
using Excel = Microsoft.Office.Interop.Excel;
using Type = Practice_bases.Models.Type;


namespace Practice_bases.Controllers;



public class HomeController : Controller
{
    private ApplicationContext _db;

    private Parser _parser;
    IWebHostEnvironment _appEnvironment;
    

    public HomeController(ApplicationContext context, IWebHostEnvironment appEnvironment)
    {
        _db = context;
        _appEnvironment = appEnvironment;
        _parser = new Parser(context);
    }
    
    [Authorize(Roles = "User, Admin")]
    public IActionResult Index()
    {
        var s = User.Identity.Name;
        return View(_db.Bases.Include(x => x.BaseRows).ToList());
    }
    

    [Authorize(Roles = "Admin")]
    public IActionResult CreateBase()
    {
        return View(new BaseViewModel()
        {
            Base = new Base()
        });
    }
    
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ImportFile(BaseViewModel BaseViewModel)
    {
        try
        {
            if (BaseViewModel.uploadedFile != null)
            {
                string path = "/ExcelFiles/" + BaseViewModel.uploadedFile.FileName;
                using (var fileStream = new FileStream(
                           _appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    BaseViewModel.uploadedFile.CopyToAsync(fileStream);
                    openExcel(_appEnvironment.WebRootPath + path, BaseViewModel.Base);
                }
            }
        }
        catch (Exception e)
        {
                
        }
        return RedirectToAction("Index");
    }
    
    
    [Authorize(Roles = "Admin")]
    void openExcel(string path, Base _base)
    {
        Excel.Application ObjExcel = new Excel.Application();
        //Открываем книгу.                                                                                                                                                        
        Excel.Workbook ObjWorkBook = ObjExcel.Workbooks.Open(
            path,
            0, false, 5, "",
            "", false,
            Excel.XlPlatform.xlWindows, "", true,
            false, 0, true, false,
            false);

        //Выбираем таблицу(лист).
        Excel.Worksheet  ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];
        
        //Excel.Range RowTitle = ObjWorkSheet.UsedRange.Rows[0];
        //string[] stringsTitle = RowTitle.OfType<object>().Select(o => o.ToString()).ToArray();

       
        Excel.Range Rows = (Excel.Range) ObjWorkSheet.UsedRange.Rows;

        _base.BaseRows = new List<BaseRow>();

        for (int i = 2; i < Rows.Count; i++)
        {
            Excel.Range usedColumn = (Excel.Range) Rows[i];
            //строка со значениями
            Array values = usedColumn.Cells.Value2;
            ArrayList objs = new ArrayList();

            foreach (var value in values)
            {
                objs.Add(value);
            }
            
            Human Student = _parser.parseFIO(objs[1], Type.Student);
            Group Group = _parser.parseGroup(objs[2]);
            Organization Organization = _parser.ParceOrganization(objs[3]);
            
            Human SupervisorCol = _parser.parseFIO(objs[4], Type.College);
            Human SupervisorOrg = _parser.parseFIO(objs[5], Type.Organization);
            
            Mail MailSupervisorCol = _parser.ParceMail(objs[6], Type.College);
            Mail MailSupervisorOrg = _parser.ParceMail(objs[7], Type.Organization);
            
            Phone PhoneCol = _parser.ParcePhone(objs[8], Type.College);
            Phone PhoneOrg = _parser.ParcePhone(objs[9], Type.Organization);
           
            Address Address = _parser.ParceAddress(objs[10]);
            Website Website = _parser.ParceWebsite(objs[11]);

            BaseRow baseRow = new BaseRow()
            {
                Student = Student,
                SupervisorCol = new Supervisor()
                {
                    Human = SupervisorCol,
                    Mail = MailSupervisorCol,
                    Phone = PhoneCol
                },
                SupervisorOrg = new Supervisor()
                {
                    Human = SupervisorOrg,
                    Mail = MailSupervisorOrg,
                    Phone = PhoneOrg
                },
                Group = Group,
                OrganizationPlus = new OrganizationPlus()
                {
                    Address = Address,
                    Organization = Organization,
                    Website = Website
                }
            };
            
            _base.BaseRows.Add(baseRow);
            _db.BaseRows.Add(baseRow);
            _db.SaveChanges();
        }

        _db.Bases.Add(_base);
        _db.SaveChanges();
        ObjExcel.Quit();
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}