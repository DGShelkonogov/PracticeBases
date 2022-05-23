using System.ComponentModel.DataAnnotations;

namespace Practice_bases.Models;

public class BaseRow
{
    [Key]
    public int Id { get; set; }
    
    public Human Student { get; set; }
    
    //public Human SupervisorCol { get; set; }
    public Supervisor SupervisorCol { get; set; }
    
    //public Human SupervisorOrg { get; set; }
    public Supervisor SupervisorOrg { get; set; }
    
    public Group Group { get; set; }
    
   // public Mail MailSupervisorCol { get; set; }
    
    //public Mail MailSupervisorOrg { get; set; }
    
    //public Phone PhoneCol { get; set; }
    
    //public Phone PhoneOrg { get; set; }
    
    //public Address Address { get; set; }
    
    //public Organization Organization { get; set; }
    
    public OrganizationPlus OrganizationPlus { get; set; }
    
    //public Website Website { get; set; }
    
    
    
}