﻿using Api.Enums;
using Api.Features.Tasks.Entities;
using Api.Features.Tasks.Models;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Features.SubTasks.Entities
{
    public class SubTask : ISubTask
    {
        public Guid Id { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public Guid TaskId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public Task Task { get; set; }

        public string Title { get; set; }

        public Priority? Priority { get; set; }

        public bool? Completed { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
