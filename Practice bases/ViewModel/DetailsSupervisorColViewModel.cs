using Practice_bases.Models;

namespace Practice_bases.ViewModel;

public class DetailsSupervisorColViewModel
{
    public int Id { get; set; }
    
    public Phone PhoneCol { get; set; }
    
    public Human SupervisorCol { get; set; }
    
    public Mail MailSupervisorCol { get; set; }
    
    public bool  PhoneColNull { get; set; }
    public bool  SupervisorColNull { get; set; }
    public bool  MailSupervisorColNull { get; set; }
    
    public bool isAdmin { get; set; }
}