using Api.Enums;
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
        public string Name { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Description { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Domain { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public Priority? Priority { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTimeOffset StartDate { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTimeOffset EndDate { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public bool? Completed { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime CreatedAt { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime UpdatedAt { get; set; }
    }
}
