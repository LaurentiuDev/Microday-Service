using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Data.Entities.Authentication
{
    public class UserLogin
    {
        [DisplayName("Email"), MaxLength(256)]
        [Required(ErrorMessage = "Please enter your email address.")]

        public string Email { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Please enter a password."), MaxLength(32)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
