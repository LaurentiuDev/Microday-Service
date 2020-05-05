using Api.Features.BaseRepository.Interfaces;
using Api.Features.Tasks.Entities;
using Api.Features.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Features.Tasks.Repositories
{
    public interface ITaskRepository: IBaseRepository<Task>
    {
    }
}
