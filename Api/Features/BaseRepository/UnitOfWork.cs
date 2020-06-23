using Api.DataAccess;
using Api.Features.BaseRepository.Interfaces;
using Api.Features.SubTasks.Repositories;
using Api.Features.Tasks.Repositories;
using Api.Features.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.BaseRepository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public ITaskRepository Tasks { get; private set; }

        public ISubTaskRepository SubTasks { get; private set; }

        public IUserRepository Users { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Tasks = new TaskRepository(_dbContext);
            SubTasks = new SubTaskRepository(_dbContext);
            Users = new UserRepository(_dbContext);
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
