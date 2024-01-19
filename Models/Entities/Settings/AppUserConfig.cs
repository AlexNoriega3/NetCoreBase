using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Enums;

namespace Models.Entities.Settings
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUsuario>
    {
        public void Configure(EntityTypeBuilder<AppUsuario> builder)
        {
            builder.Property(e => e.Active)
                    .HasDefaultValueSql("1");

            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Property(e => e.Gender)
                    .HasDefaultValue(GenderEnum.Otro)
                    .HasColumnType("varchar(10)")
                    .HasConversion<string>();

            builder.Property(e => e.Name)
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100);

            builder.Property(e => e.TitleAbbreviation)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50);
        }
    }
}