using Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Tasks.Entities
{
    public interface ITask
    {
        Guid Id { get; set; }

        string UserId { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        string Domain { get; set; }

        Priority? Priority { get; set; }

        DateTimeOffset StartDate { get; set; }

        DateTimeOffset EndDate { get; set; }

        bool? Completed { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
