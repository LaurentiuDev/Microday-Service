using Api.Features.Authentication.Entities;
using Api.Features.BaseRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Users.Repositories
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
    }
}
