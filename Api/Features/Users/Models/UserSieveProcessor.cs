using Api.Features.Authentication.Entities;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Users.Models
{
    public class UserSieveProcessor : SieveProcessor
    {
        public UserSieveProcessor(IOptions<SieveOptions> optionsParameters)
            : base(optionsParameters)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<ApplicationUser>(x => x.FirstName).CanSort().CanFilter();
            mapper.Property<ApplicationUser>(x => x.LastName).CanSort().CanFilter();
            mapper.Property<ApplicationUser>(x => x.Email).CanSort().CanFilter();

            return mapper;
        }
    }
}
