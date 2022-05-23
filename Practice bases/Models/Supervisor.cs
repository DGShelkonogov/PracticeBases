using System.ComponentModel.DataAnnotations;

namespace Practice_bases.Models;

public class Supervisor
{
  [Key]
  public int Id { get; set; }
  public Human Human { get; set; }
  public Phone Phone { get; set; }
  public Mail Mail { get; set; }
  
}