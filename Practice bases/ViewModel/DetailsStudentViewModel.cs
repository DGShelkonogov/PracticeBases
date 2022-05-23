using System.Collections;
using Practice_bases.Models;

namespace Practice_bases.ViewModel;

public class DetailsStudentViewModel
{
    public int Id { get; set; }

    public BaseRow BaseRow { get; set; }

    public List<Group> Groups { get; set; }
    
    public List<Human> SupervisorsCol { get; set; }
    
    public List<Human> SupervisorsOrg { get; set; }
    
    public List<Organization> Organizations { get; set; }
    
    public bool isAdmin { get; set; }
}