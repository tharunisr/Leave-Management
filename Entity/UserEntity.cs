using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveManagement.Entity
{
    [Table("Users")]
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name can't be blank")]
        [RegularExpression(@"^[A-Za-z ]*$", ErrorMessage = "Alphabets only")]
        [MaxLength(50, ErrorMessage = "Max length should be 50")]
        [MinLength(2, ErrorMessage = "Atleast 2 character")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User Type is required")]
        [Display(Name="UserType")]
        public string UserType { get; set; }
       
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        

        [Required(ErrorMessage = "Password can't be blank")]
        public string Password { get; set; }

    
        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [Compare("Password", ErrorMessage = "Should be same as Password ")]
        [Display(Name="Confirm Password")]
        public string ConfirmPassword { get; set; }

        public bool IsDeleted { get; set; }
        public int UserTypeId { get; set; }
        public ICollection<LeaveEntity> Leaves { get; set; }

    }
}