using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Data.Entities.Authentication
{
    public class UserRegistration : UserLogin
    {
        [DisplayName("Username"), MaxLength(256)]
        [Required(ErrorMessage = "Please enter your username.")]
        public string UserName { get; set; }

        [DisplayName("First name")]
        [Required(ErrorMessage = "Please enter your first name."), MaxLength(256)]
        public string FirstName { get; set; }

        [DisplayName("Last Name"), MaxLength(256)]
        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        [Required(ErrorMessage = "Please confirm your password."), MaxLength(32)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
