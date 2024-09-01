using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name", TypeName = "Varchar(100)")]
        public string ProductName { get; set; }

        public float Price { get; set; }

        [Column(TypeName = "VARBINARY(MAX)")]
        public byte[] ImageData { get; set; }

        [Column("PreparationTime")]
        public string Time { get; set; }
    }


}
