using Api.Features.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Authentication.Models
{
    public class LoginResultVM
    {
		public bool Status { get; set; }
		public string Medium { get; set; }
		public string Platform { get; set; }

		public ApplicationUser User { get; set; }

		public string Error { get; set; }
		public string ErrorDescription { get; set; }

		public string Token { get; set; }
	}
}
