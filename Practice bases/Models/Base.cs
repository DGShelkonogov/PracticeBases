using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_bases.Models;
public enum Type
{
    Student,
    College,
    Organization
}

public class Base
{
    [Key]
    public int Id { get; set; }

    public string Specialty { get; set; }
    
    [Column(TypeName = "Date")]
    public DateTime StartDate { get; set; }
    
    [Column(TypeName = "Date")]
    public DateTime EndDate { get; set; }
    
    public virtual ICollection<BaseRow> BaseRows { get; set; }
}