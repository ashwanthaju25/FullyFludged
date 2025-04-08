using System.ComponentModel.DataAnnotations;

namespace FullyFludged.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
    }
}
