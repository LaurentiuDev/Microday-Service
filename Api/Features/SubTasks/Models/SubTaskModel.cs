using Api.Constants;
using Api.Enums;
using Api.Features.SubTasks.Entities;
using Api.Features.Tasks.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Api.Features.SubTasks.Models
{
    public class SubTaskModel : ISubTask
    {
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }

        public Task Task { get; set; }

        [Required]
        [MaxLength(DataAnnotationConstants.MAX_LENGTH_512)]
        public string Title { get; set; }

        public Priority? Priority { get; set; }

        public bool? Completed { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
