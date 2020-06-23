using Api.Features.Authentication.Entities;
using Api.Features.BaseRepository;
using Api.Features.BaseRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Users.Services
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return _unitOfWork.Users.GetAll();
        }

        public ApplicationUser Get(Guid id)
        {
            return _unitOfWork.Users.Get(id);
        }

        public void Add(ApplicationUser entity)
        {
            _unitOfWork.Users.Add(entity);
        }

        public void Update(ApplicationUser entity)
        {
            _unitOfWork.Users.Update(entity);
        }

        public void Delete(ApplicationUser entity)
        {
            _unitOfWork.Users.Delete(entity);
        }

        public bool Exists(string id)
        {
            return _unitOfWork.Users.Exists(p => p.Id == id);
        }
    }
}
