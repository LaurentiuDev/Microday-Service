using Api.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.SubTasks.Entities
{
    public class SubTaskConfiguration : IEntityTypeConfiguration<SubTask>
    {
        public void Configure(EntityTypeBuilder<SubTask> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(DataAnnotationConstants.MAX_LENGTH_512)
                .IsRequired(true);

            builder.Property(x => x.TaskId)
                .IsRequired(true);

            // FK - configuration
            builder.HasOne(x => x.Task).WithMany(x => x.SubTasks).HasForeignKey(x => x.TaskId).IsRequired(true);
        }
    }
}
