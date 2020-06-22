using Api.Features.SubTasks.Entities;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.SubTasks.Models
{
    public class SubTaskSieveProcessor : SieveProcessor
    {
        public SubTaskSieveProcessor(IOptions<SieveOptions> optionsParameters)
            : base(optionsParameters)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<SubTask>(x => x.TaskId).CanSort().CanFilter();
            mapper.Property<SubTask>(x => x.Task).CanSort().CanFilter();
            mapper.Property<SubTask>(x => x.Title).CanSort().CanFilter();
            mapper.Property<SubTask>(x => x.Priority).CanSort().CanFilter();
            mapper.Property<SubTask>(x => x.Completed).CanSort().CanFilter();
            mapper.Property<SubTask>(x => x.CreatedAt).CanSort().CanFilter();
            mapper.Property<SubTask>(x => x.UpdatedAt).CanSort().CanFilter();

            return mapper;
        }
    }
}
