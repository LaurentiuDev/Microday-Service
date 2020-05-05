using Api.DataAccess;
using Api.Features.BaseRepository;
using Api.Features.Tasks.Entities;
using Api.Features.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Features.Tasks.Repositories
{
    public class TaskRepository : BaseRepository<Task>, ITaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
    }
}
