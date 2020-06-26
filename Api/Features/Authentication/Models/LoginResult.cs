using Api.Features.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Authentication.Models
{
    public class LoginResult
    {
		public bool Status { get; set; }
		public string Platform { get; set; }

		public ApplicationUser User { get; set; }
		public string Token { get; set; }

		public string Error { get; set; }
		public string ErrorDescription { get; set; }
	}
}
