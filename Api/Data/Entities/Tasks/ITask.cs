using Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Entities.Tasks
{
    public interface ITask
    {
        string Name { get; set; }

        string Domain { get; set; }

        Priority Priority { get; set; }

        int Duration { get; set; }

        DateTime StartDate { get; set; }

        DateTime EndDate { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
