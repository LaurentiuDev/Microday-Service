using Api.Features.Tasks.Entities;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Features.Tasks.Models
{
    public class TaskSieveProcessor : SieveProcessor
    {
        public TaskSieveProcessor(IOptions<SieveOptions> optionsParameters)
            : base(optionsParameters)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<Task>(x => x.UserId).CanSort().CanFilter();
            mapper.Property<Task>(x => x.User).CanSort().CanFilter();
            mapper.Property<Task>(x => x.Name).CanSort().CanFilter();
            mapper.Property<Task>(x => x.Description).CanSort().CanFilter();
            mapper.Property<Task>(x => x.Domain).CanSort().CanFilter();
            mapper.Property<Task>(x => x.Priority).CanSort().CanFilter();
            mapper.Property<Task>(x => x.StartDate).CanSort().CanFilter();
            mapper.Property<Task>(x => x.EndDate).CanSort().CanFilter();
            mapper.Property<Task>(x => x.Completed).CanSort().CanFilter();
            mapper.Property<Task>(x => x.CreatedAt).CanSort().CanFilter();
            mapper.Property<Task>(x => x.UpdatedAt).CanSort().CanFilter();

            return mapper;
        }
    }
}
