using Api.Features.SubTasks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.SubTasks.Services
{
    public interface ISubTaskService
    {
        IQueryable<SubTask> GetAll();

        SubTask Get(Guid id);

        void Add(SubTask entity);

        void Update(SubTask entity);

        void Delete(SubTask entity);

        bool Exists(Guid id);
    }
}
