using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class OtherAccountException : Exception
	{
		public OtherAccountException(IList<UserLoginInfo> existing_logins)
			: base($"Please login with your {existing_logins[0].ProviderDisplayName} account instead")
		{
		}
	}
}
