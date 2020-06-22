using Api.Features.BaseRepository.Interfaces;
using Api.Features.SubTasks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.SubTasks.Repositories
{
    public interface ISubTaskRepository : IBaseRepository<SubTask>
    {
    }
}
