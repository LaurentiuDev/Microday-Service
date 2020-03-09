using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class UserForgotPassword
    {
        [DisplayName("Email"), MaxLength(256)]
        [Required(ErrorMessage = "Please enter your email address.")]

        public string Email { get; set; }
    }
}
