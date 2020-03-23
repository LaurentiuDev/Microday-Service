using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Authentication.Models
{
    public class UserForgotPassword
    {
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }
    }
}
