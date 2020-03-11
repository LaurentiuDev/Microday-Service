using Api.Data.Entities.Tasks;
using Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Tasks.Models
{
    public class TaskModel : ITask
    {
        public string Name { get; set; }

        public string Domain { get; set; }

        public Priority Priority { get; set; }

        public int Duration { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
