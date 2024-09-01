using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Foodie.Models
{
    public class ProductViewModel { 
    
        public int Id {  get; set; }
        [Required]
        public string ProductName { get; set; }

        [Required]
        public float Price { get; set; }

        public IFormFile Image { get; set; }

        [Required]
        public string PreparationTime { get; set; }
    }
}
