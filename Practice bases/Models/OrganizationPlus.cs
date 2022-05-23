using System.ComponentModel.DataAnnotations;

namespace Practice_bases.Models;

public class OrganizationPlus
{
    [Key]
    public int Id { get; set; }
    public Organization Organization { get; set; }
    public Address Address { get; set; }
    public Website Website { get; set; }
}