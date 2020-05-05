using Api.Features.Tasks.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Features.Tasks.Models
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Task, TaskModel>();

            CreateMap<TaskModel, Task>();
        }
    }
}
