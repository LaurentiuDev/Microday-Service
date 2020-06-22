using Api.Features.SubTasks.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.SubTasks.Models
{
    public class SubTaskProfile : Profile
    {
        public SubTaskProfile()
        {
            CreateMap<SubTask, SubTaskModel>();

            CreateMap<SubTaskModel, SubTask>();
        }
    }
}
