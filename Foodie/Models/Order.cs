using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column( TypeName = "Varchar(100)")]

        public string CFName { get; set; }
        [Column( TypeName = "Varchar(100)")]

        public string CLname { get; set; }
        [Column( TypeName = "Varchar(100)")]

        public string Address {  get; set; }
        [Column( TypeName = "Varchar(100)")]
        public string Contact { get; set; }
        [Column(TypeName = "Varchar(100)")]
        public string ProductName {  get; set; }
        public int Qty {  get; set; }
        public float price {  get; set; }
        public int OrderStatus { get; set; } 
        



    }
}
