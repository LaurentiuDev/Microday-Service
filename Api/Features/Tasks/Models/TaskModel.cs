using Api.Constants;
using Api.Enums;
using Api.Features.Tasks.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Features.Tasks.Models
{
    public class TaskModel : ITask
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataAnnotationConstants.MAX_LENGTH_64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataAnnotationConstants.MAX_LENGTH_512)]
        public string Description { get; set; }

        [Required]
        [MaxLength(DataAnnotationConstants.MAX_LENGTH_64)]
        public string Domain { get; set; }

        public Priority Priority { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
