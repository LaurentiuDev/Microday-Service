using Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.SubTasks.Entities
{
    public interface ISubTask
    {
        Guid Id { get; set; }

        Guid TaskId { get; set; }

        string Title { get; set; }

        Priority? Priority { get; set; }

        bool? Completed { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
