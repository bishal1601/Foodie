using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int  ProductId { get; set; }
        [Column(TypeName = "Varchar(100)")]
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public float Price { get; set; }
        
    }
}
