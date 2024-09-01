using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("FName", TypeName = "Varchar(100)")]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required]
        [Column("LName", TypeName = "Varchar(100)")]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required]
        [Column("UserName", TypeName = "Varchar(100)")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Column("Password", TypeName = "Varchar(max)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Column("Email", TypeName = "Varchar(100)")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column("Contact", TypeName = "Varchar(100)")]
        [Phone]
        public string Contact { get; set; }
    }
}
