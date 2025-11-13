using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN_PE.Models;

namespace PRN_PE.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            // Configure table and add a check constraint for rating range
            builder.ToTable("posts", t =>
                t.HasCheckConstraint("CK_Posts_Rating_Range", "(Rating IS NULL) OR (Rating >= 1.00 AND Rating <= 5.00)"));
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.ImageUrl).HasMaxLength(1000);
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.IsDeleted).HasDefaultValue(false);
            // Rating stored as DECIMAL(3,2), nullable when there are no ratings yet
            builder.Property(p => p.Rating)
                .HasColumnType("decimal(3,2)")
                .IsRequired(false);
        }
    }
}