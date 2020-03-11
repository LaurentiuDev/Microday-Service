using Api.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Entities.Tasks
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(DataAnnotationConstants.MAX_LENGTH_32)
                .IsRequired(true);

            builder.Property(x => x.Domain)
                .HasMaxLength(DataAnnotationConstants.MAX_LENGTH_32)
                .IsRequired(true);

        }
    }
}
