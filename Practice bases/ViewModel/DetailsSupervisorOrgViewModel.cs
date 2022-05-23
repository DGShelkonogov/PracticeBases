using Practice_bases.Models;

namespace Practice_bases.ViewModel;

public class DetailsSupervisorOrgViewModel
{
    public int Id { get; set; }
    
    public Phone PhoneOrg { get; set; }
    
    public Human SupervisorOrg { get; set; }
    
    public Mail MailSupervisorOrg { get; set; }
    
    public bool  PhoneOrgNull { get; set; }
    public bool  SupervisorOrgNull { get; set; }
    public bool  MailSupervisorOrgNull { get; set; }

    public bool isAdmin { get; set; }
}