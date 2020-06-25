using Api.Enums;
using Api.Features.Authentication.Entities;
using Api.Features.SubTasks.Entities;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Tasks.Entities
{
    public class Task : ITask
    {
        public Guid Id { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string UserId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public ApplicationUser User { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Domain { get; set; }

        public Priority? Priority { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public bool? Completed { get; set; }

        public IList<SubTask> SubTasks { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
