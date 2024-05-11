using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Products.Models
{
    public class ProductDto
    {
        [Required,MaxLength(100)]
        public string Name { get; set; } = "";

        [Required,MaxLength(100)]
        public string Brand { get; set; } = "";

        [Required, MaxLength(100)]
        public string Category { get; set; } = "";

        [Required] 
        public int price { get; set; }
        [Required]
        public string Discription { get; set; } = "";


        public IFormFile? ImagFile { get; set; }
    }
}
