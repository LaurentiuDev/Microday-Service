using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Data.Entities.Authentication
{
    public class UserForgotPassword
    {
        [DisplayName("Email"), MaxLength(256)]
        [Required(ErrorMessage = "Please enter your email address.")]

        public string Email { get; set; }
    }
}
