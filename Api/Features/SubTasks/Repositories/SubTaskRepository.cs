using Api.DataAccess;
using Api.Features.BaseRepository;
using Api.Features.SubTasks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.SubTasks.Repositories
{
    public class SubTaskRepository : BaseRepository<SubTask>, ISubTaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SubTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
    }
}
