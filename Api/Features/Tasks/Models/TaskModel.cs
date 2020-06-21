using Api.Constants;
using Api.Enums;
using Api.Features.SubTasks.Entities;
using Api.Features.Tasks.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Features.Tasks.Models
{
    public class TaskModel : ITask
    {
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(DataAnnotationConstants.MAX_LENGTH_64)]
        public string Name { get; set; }

        [MaxLength(DataAnnotationConstants.MAX_LENGTH_512)]
        public string Description { get; set; }

        [Required]
        [MaxLength(DataAnnotationConstants.MAX_LENGTH_64)]
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
