using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_bases.Models;
using Practice_bases.ViewModel;
using Type = Practice_bases.Models.Type;

namespace Practice_bases.Controllers;


[Authorize]
public class SearchController : Controller
{
    private ApplicationContext _db;
    private User _user;
    
    
    public SearchController(ApplicationContext context)
    {
        _db = context;
    }
    
    
    
    public IActionResult SearchStudent()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Search(string name, string number)
    {
        try
        {
            var bases = new List<BaseRow>();
            switch (number)
            {
                case "1":
                     bases = _db.BaseRows
                        .Include(x => x.Student)
                        .Include(x => x.Group)
                        .Include(x => x.OrganizationPlus.Address)
                        .Include(x => x.OrganizationPlus.Organization)
                        .Include(x => x.SupervisorCol.Human)
                        .Include(x => x.SupervisorOrg.Human)
                        .Where(x => x.Student.Name.ToLower().Contains(name.ToLower())).Distinct().ToList();
                    return View("SearchStudent", bases);
                case "2":
                    bases = _db.BaseRows
                        .Include(x => x.SupervisorCol)
                        .ThenInclude(x => x.Phone)
                        .Include(x => x.SupervisorCol)
                        .ThenInclude(x => x.Human)
                        .Where(x => x.SupervisorCol.Human.Name.ToLower().Contains(name.ToLower())).Distinct().ToList();
                    return View("SearchSupervisorCol", bases);
                case "3":
                    bases = _db.BaseRows
                        .Include(x => x.SupervisorOrg)
                        .ThenInclude(x => x.Phone)
                        .Include(x => x.SupervisorOrg)
                        .ThenInclude(x => x.Human)
                        .Where(x => x.SupervisorOrg.Human.Name.ToLower().Contains(name.ToLower())).Distinct().ToList();
                    return View("SearchSupervisorOrg", bases);
                case "4":
                    bases = _db.BaseRows
                        .Include(x => x.OrganizationPlus)
                        .ThenInclude(x => x.Organization)
                        .Where(x => x.OrganizationPlus.Organization.Title.ToLower().Contains(name.ToLower())).Distinct().ToList();
                    return View("SearchOrganization", bases);
            }
        }
        catch (Exception e)
        {
                
        }
        return View("SearchStudent", null);
    }
    
    public async Task<IActionResult> DetailsStudentInfo(int Id)
    {
        
        var baseRow = _db.BaseRows
            .Include(x => x.Student)
            .Include(x => x.Group)
            .Include(x => x.OrganizationPlus.Address)
            .Include(x => x.OrganizationPlus.Organization)
            .Include(x => x.OrganizationPlus.Website)
            .Include(x => x.SupervisorCol.Phone)
            .Include(x => x.SupervisorOrg.Phone)
            .Include(x => x.SupervisorCol)
            .Include(x => x.SupervisorOrg)
            .Include(x => x.SupervisorCol.Mail)
            .Include(x => x.SupervisorOrg.Mail)
            .FirstOrDefault(x => x.Id == Id);

        if (baseRow == null) 
            return View();
        

        DetailsStudentViewModel model = new DetailsStudentViewModel()
        {
            BaseRow = baseRow,
            Groups = _db.Groups.ToList(),
            Organizations = _db.Organizations.ToList(),
            SupervisorsCol = _db.Humans.Where(x => x.Type == Type.College).ToList(),
            SupervisorsOrg = _db.Humans.Where(x => x.Type == Type.Organization).ToList(),
            isAdmin = _user != null && (_user.Role == Role.ADMIN)
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DetailsStudentInfo(DetailsStudentViewModel model)
    {
        var Group = _db.Groups.FirstOrDefault(x => x.Title.Equals(Request.Form["selectedGroup"]));
        
        var SupervisorOrg = _db.Supervisors
            .Include(x => x.Human)
            .Include(x => x.Mail)
            .Include(x => x.Phone)
            .FirstOrDefault(x => 
                x.Human.Name.Equals(Request.Form["selectedSupervisorOrg"]) 
                                && x.Human.Type == Type.Organization);
        
        
        var SupervisorCol = _db.Supervisors
            .Include(x => x.Human)
            .Include(x => x.Mail)
            .Include(x => x.Phone)
            .FirstOrDefault(x => x.Human.Name.Equals(Request.Form["selectedSupervisorCol"])
                                 && x.Human.Type == Type.College);
        
        
        var Organization = _db.OrganizationsPlus
            .Include(x => x.Organization)
            .Include(x => x.Website)
            .Include(x => x.Address)
            .FirstOrDefault(x => x.Organization.Title.Equals(Request.Form["selectedOrganization"]));
        
        
        
        var baseRow = _db.BaseRows.FirstOrDefault(x => x.Id == model.Id);

        baseRow.Group = Group;
        baseRow.SupervisorOrg = SupervisorOrg;
        baseRow.SupervisorCol = SupervisorCol;
        baseRow.OrganizationPlus = Organization;
        //baseRow.Student.Name = baseRow.Student.Name;

        await _db.SaveChangesAsync();
        
        return RedirectToAction("DetailsStudentInfo", model.Id);
    }
    
    
    
    public async Task<IActionResult> DetailsOrganization(int Id)
    {
        var baseRow = _db.BaseRows
            .Include(x => x.OrganizationPlus.Address)
            .Include(x => x.OrganizationPlus.Organization)
            .Include(x => x.OrganizationPlus.Website)
            .FirstOrDefault(x => x.Id == Id);

        DetailsOrganizationViewModel model = new DetailsOrganizationViewModel()
        {
            Id = Id,
            Address = baseRow.OrganizationPlus.Address,
            Organization = baseRow.OrganizationPlus.Organization,
            Website = baseRow.OrganizationPlus.Website,
            isAdmin = _user != null && _user.Role == Role.ADMIN
        };


        model.AddressNull = baseRow.OrganizationPlus.Address.Title.ToLower().Equals("не указано");
        model.OrganizationNull = baseRow.OrganizationPlus.Organization.Title.ToLower().Equals("не указано");
        model.WebsiteNull = baseRow.OrganizationPlus.Website.Title.ToLower().Equals("не указано");
        
        
        if (baseRow != null)
            return View(model);
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> DetailsOrganization(DetailsOrganizationViewModel model)
    {
        var organization = _db.Organizations.FirstOrDefault(x => x.Id == model.Organization.Id);
        var orgs = _db.BaseRows.Include(x => x.OrganizationPlus.Organization)
            .Where(x => x.OrganizationPlus.Organization.Title.ToLower().Equals(organization.Title.ToLower()));
        
        //если организация новоя
        if (model.OrganizationNull && !model.Organization.Title.ToLower().Equals("не указано"))
        {
            organization = new Organization()
            {
                Title = model.Organization.Title
            };
            _db.Organizations.Add(organization);
        }
        else
        {
            organization.Title = model.Organization.Title;
            _db.Organizations.Update(organization);
        }

        //если адресс новоя
        if (model.AddressNull && !model.Address.Title.ToLower().Equals("не указано"))
        {
            var address = new Address()
            {
                Title = model.Address.Title
            };
            _db.Address.Add(address);

            foreach (var item in orgs)
                item.OrganizationPlus.Address = address;
        }
        else
        {
            var address = _db.Address.FirstOrDefault(x => x.Id == model.Address.Id);
            address.Title = model.Address.Title;
            _db.Address.Update(address);
        }

        
        if (model.WebsiteNull && !model.Website.Title.ToLower().Equals("не указано"))
        {
            var website = new Website()
            {
                Title = model.Website.Title
            };
            _db.Websites.Add(website);
            
            foreach (var item in orgs)
                item.OrganizationPlus.Website = website;
        }
        else
        {
            var website = _db.Websites.FirstOrDefault(x => x.Id == model.Website.Id);
            website.Title = model.Website.Title;
            _db.Websites.Update(website);
        }
        
         await _db.SaveChangesAsync();
         return RedirectToAction("DetailsOrganization", model.Id);
    }
    
    
    
    public async Task<IActionResult> DetailsSupervisorCol(int Id)
    {
        var baseRow = _db.BaseRows
            .Include(x => x.SupervisorCol.Phone)
            .Include(x => x.SupervisorCol.Human)
            .Include(x => x.SupervisorCol.Mail)
            .FirstOrDefault(x => x.Id == Id);
        
        if (baseRow == null)
            return NotFound();

        DetailsSupervisorColViewModel model = new DetailsSupervisorColViewModel()
        {
            Id = Id,
            PhoneCol = baseRow.SupervisorCol.Phone,
            SupervisorCol = baseRow.SupervisorCol.Human,
            MailSupervisorCol = baseRow.SupervisorCol.Mail,
            isAdmin = _user != null && _user.Role == Role.ADMIN
        };
        
        model.PhoneColNull = baseRow.SupervisorCol.Phone.Title.ToLower().Equals("не указано");
        model.SupervisorColNull = baseRow.SupervisorCol.Human.Name.ToLower().Equals("не указано");
        model.MailSupervisorColNull = baseRow.SupervisorCol.Mail.Title.ToLower().Equals("не указано");
        
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> DetailsSupervisorCol(DetailsSupervisorColViewModel model)
    {
        var supervisorsCol = _db.Humans.FirstOrDefault(x => x.Id == model.SupervisorCol.Id);
        var rows = _db.BaseRows.Include(x => x.SupervisorCol)
            .Where(x => x.SupervisorCol.Human.Name.ToLower().Equals(supervisorsCol.Name.ToLower()));
        
        //если организация новоя
        if (model.SupervisorColNull && !model.SupervisorCol.Name.ToLower().Equals("не указано"))
        {
            supervisorsCol = new Human()
            {
                Name = model.SupervisorCol.Name,
                Type = Type.College
            };
            _db.Humans.Add(supervisorsCol);
        }
        else
        {
            supervisorsCol.Name = model.SupervisorCol.Name;
            _db.Humans.Update(supervisorsCol);
        }

        //если адресс новоя
        if (model.PhoneColNull && !model.PhoneCol.Title.ToLower().Equals("не указано"))
        {
            var phone = new Phone()
            {
                Title = model.PhoneCol.Title
            };
            _db.Phones.Add(phone);

            foreach (var item in rows)
                item.SupervisorCol.Phone = phone;
        }
        else
        {
            var phone = _db.Phones.FirstOrDefault(x => x.Id == model.PhoneCol.Id);
            phone.Title = model.PhoneCol.Title;
            _db.Phones.Update(phone);
        }

        
        if (model.MailSupervisorColNull && !model.MailSupervisorCol.Title.ToLower().Equals("не указано"))
        {
            var mail = new Mail()
            {
                Title = model.MailSupervisorCol.Title
            };
            _db.Mails.Add(mail);
            
            foreach (var item in rows)
                item.SupervisorCol.Mail = mail;
        }
        else
        {
            var mail = _db.Mails.FirstOrDefault(x => x.Id == model.MailSupervisorCol.Id);
            mail.Title = model.MailSupervisorCol.Title;
            _db.Mails.Update(mail);
        }
        
         await _db.SaveChangesAsync();
         return RedirectToAction("DetailsSupervisorCol", model.Id);
    }
    
    
    
    public async Task<IActionResult> DetailsSupervisorOrg(int Id)
    {
        var baseRow = _db.BaseRows
            .Include(x => x.SupervisorOrg.Phone)
            .Include(x => x.SupervisorOrg.Human)
            .Include(x => x.SupervisorOrg.Mail)
            .FirstOrDefault(x => x.Id == Id);
        
        if (baseRow == null)
            return NotFound();
        
        DetailsSupervisorOrgViewModel model = new DetailsSupervisorOrgViewModel()
        {
            Id = Id,
            PhoneOrg = baseRow.SupervisorOrg.Phone,
            SupervisorOrg = baseRow.SupervisorOrg.Human,
            MailSupervisorOrg = baseRow.SupervisorOrg.Mail,
            isAdmin = _user != null && _user.Role == Role.ADMIN
        };
        
        model.PhoneOrgNull = baseRow.SupervisorOrg.Phone.Title.ToLower().Equals("не указано");
        model.SupervisorOrgNull = baseRow.SupervisorOrg.Human.Name.ToLower().Equals("не указано");
        model.MailSupervisorOrgNull = baseRow.SupervisorOrg.Mail.Title.ToLower().Equals("не указано");
        
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> DetailsSupervisorOrg(DetailsSupervisorOrgViewModel model)
    {
        var supervisorsOrg = _db.Humans.FirstOrDefault(x => x.Id == model.SupervisorOrg.Id);
        var rows = _db.BaseRows.Include(x => x.SupervisorOrg)
            .Where(x => x.SupervisorOrg.Human.Name.ToLower().Equals(supervisorsOrg.Name.ToLower()));
        
        //если организация новоя
        if (model.SupervisorOrgNull && !model.SupervisorOrg.Name.ToLower().Equals("не указано"))
        {
            supervisorsOrg = new Human()
            {
                Name = model.SupervisorOrg.Name,
                Type = Type.Organization
            };
            _db.Humans.Add(supervisorsOrg);
        }
        else
        {
            supervisorsOrg.Name = model.SupervisorOrg.Name;
            _db.Humans.Update(supervisorsOrg);
        }

        //если адресс новоя
        if (model.PhoneOrgNull && !model.PhoneOrg.Title.ToLower().Equals("не указано"))
        {
            var phone = new Phone()
            {
                Title = model.PhoneOrg.Title
            };
            _db.Phones.Add(phone);

            foreach (var item in rows)
                item.SupervisorOrg.Phone = phone;
        }
        else
        {
            var phone = _db.Phones.FirstOrDefault(x => x.Id == model.PhoneOrg.Id);
            phone.Title = model.PhoneOrg.Title;
            _db.Phones.Update(phone);
        }

        
        if (model.MailSupervisorOrgNull && !model.MailSupervisorOrg.Title.ToLower().Equals("не указано"))
        {
            var mail = new Mail()
            {
                Title = model.MailSupervisorOrg.Title
            };
            _db.Mails.Add(mail);
            
            foreach (var item in rows)
                item.SupervisorOrg.Mail = mail;
        }
        else
        {
            var mail = _db.Mails.FirstOrDefault(x => x.Id == model.MailSupervisorOrg.Id);
            mail.Title = model.MailSupervisorOrg.Title;
            _db.Mails.Update(mail);
        }
        
         await _db.SaveChangesAsync();
         return RedirectToAction("DetailsSupervisorOrg", model.Id);
    }
}