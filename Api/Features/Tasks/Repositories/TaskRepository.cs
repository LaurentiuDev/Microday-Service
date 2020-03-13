using Api.DataAccess;
using Api.Features.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Tasks.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TaskRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public void Add(TaskModel taskModel)
        {
            this._applicationDbContext.Add(taskModel);
        }

        public bool Save()
        {
            return (this._applicationDbContext.SaveChanges() >= 0);
        }
    }
}
