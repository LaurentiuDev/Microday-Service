using Api.Features.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Users.Services
{
    public interface IUserService
    {
        IQueryable<ApplicationUser> GetAll();

        ApplicationUser Get(Guid id);

        void Add(ApplicationUser entity);

        void Update(ApplicationUser entity);

        void Delete(ApplicationUser entity);

        bool Exists(string id);
    }
}
