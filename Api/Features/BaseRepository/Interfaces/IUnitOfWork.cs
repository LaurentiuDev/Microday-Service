﻿using Api.Features.SubTasks.Repositories;
using Api.Features.Tasks.Repositories;
using Api.Features.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.BaseRepository.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        ITaskRepository Tasks { get; }

        ISubTaskRepository SubTasks { get; }

        IUserRepository Users { get; }

        Task Commit();
    }
}
