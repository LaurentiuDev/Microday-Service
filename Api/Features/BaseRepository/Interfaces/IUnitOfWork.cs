﻿using Api.Features.Tasks.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.BaseRepository.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        ITaskRepository Tasks { get; }
        Task Commit();
    }
}
