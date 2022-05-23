using Practice_bases.Models;

namespace Practice_bases.ViewModel;

public class BaseViewModel
{
    public IFormFile uploadedFile { get; set; }
    
    public Base Base { get; set; }
}