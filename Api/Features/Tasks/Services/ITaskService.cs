using Api.Features.Tasks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Features.Tasks.Services
{
    public interface ITaskService
    {
        IQueryable<Task> GetAll();

        Task Get(Guid id);

        void Add(Task entity);

        void Update(Task entity);

        void Delete(Task entity);

        bool Exists(Guid id);
    }
}
