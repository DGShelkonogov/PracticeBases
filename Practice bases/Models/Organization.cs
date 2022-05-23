using System.ComponentModel.DataAnnotations;

namespace Practice_bases.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}