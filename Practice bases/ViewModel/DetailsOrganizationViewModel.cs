using Practice_bases.Models;

namespace Practice_bases.ViewModel;

public class DetailsOrganizationViewModel
{
    public int Id { get; set; }
    public Organization Organization { get; set; }
    public Address Address { get; set; }
    public Website Website { get; set; }

    public bool OrganizationNull { get; set; }
    public bool AddressNull { get; set; }
    public bool WebsiteNull { get; set; }
    
    public bool isAdmin { get; set; }
}