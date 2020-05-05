using Api.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Features.Tasks.Entities
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(DataAnnotationConstants.MAX_LENGTH_64)
                .IsRequired(true);

            builder.Property(x => x.Domain)
                .HasMaxLength(DataAnnotationConstants.MAX_LENGTH_64)
                .IsRequired(true);

            builder.Property(x => x.Description)
                .HasMaxLength(DataAnnotationConstants.MAX_LENGTH_512)
                .IsRequired(true);
        }
    }
}
