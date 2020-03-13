using Api.Features.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Tasks.Repositories
{
    public interface ITaskRepository
    {
        void Add(TaskModel entity);
        bool Save();
    }
}
