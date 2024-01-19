using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Entities.Settings
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

            builder.Property(e => e.Active)
                    .HasDefaultValueSql("1");

            builder.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("NOW()");
        }
    }
}