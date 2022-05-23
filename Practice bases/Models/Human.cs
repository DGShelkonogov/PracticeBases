using System.ComponentModel.DataAnnotations;

namespace Practice_bases.Models
{
    public class Human
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}