using Api.Features.BaseRepository;
using Api.Features.BaseRepository.Interfaces;
using Api.Features.Tasks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Features.Tasks.Services
{
    public class TaskService : ITaskService
    {
        private readonly UnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        public IQueryable<Task> GetAll()
        {
            return _unitOfWork.Tasks.GetAll();
        }

        public Task Get(Guid id)
        {
            return _unitOfWork.Tasks.Get(id);
        }

        public void Add(Task entity)
        {
            _unitOfWork.Tasks.Add(entity);
        }

        public void Update(Task entity)
        {
            _unitOfWork.Tasks.Update(entity);
        }

        public void Delete(Task entity)
        {
            _unitOfWork.Tasks.Delete(entity);
        }

        public bool Exists(Guid id)
        {
            return _unitOfWork.Tasks.Exists(p => p.Id == id);
        }
    }
}
