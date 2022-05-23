using System.ComponentModel.DataAnnotations;

namespace Practice_bases.Models;

public class Role
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}