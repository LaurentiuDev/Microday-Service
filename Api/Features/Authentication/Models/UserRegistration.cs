using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Authentication.Models
{
    public class UserRegistration : UserLogin
    {
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password), Compare(nameof(Password))]
        [MaxLength(32)]
        public string ConfirmPassword { get; set; }
    }
}
