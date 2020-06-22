using Api.Features.BaseRepository;
using Api.Features.BaseRepository.Interfaces;
using Api.Features.SubTasks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.SubTasks.Services
{
    public class SubTaskService : ISubTaskService
    {
        private readonly UnitOfWork _unitOfWork;

        public SubTaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        public IQueryable<SubTask> GetAll()
        {
            return _unitOfWork.SubTasks.GetAll();
        }

        public SubTask Get(Guid id)
        {
            return _unitOfWork.SubTasks.Get(id);
        }

        public void Add(SubTask entity)
        {
            _unitOfWork.SubTasks.Add(entity);
        }

        public void Update(SubTask entity)
        {
            _unitOfWork.SubTasks.Update(entity);
        }

        public void Delete(SubTask entity)
        {
            _unitOfWork.SubTasks.Delete(entity);
        }

        public bool Exists(Guid id)
        {
            return _unitOfWork.SubTasks.Exists(p => p.Id == id);
        }
    }
}
