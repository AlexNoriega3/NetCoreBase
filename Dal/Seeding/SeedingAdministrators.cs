using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Dal.Seeding
{
    public static class SeedingAdministrators
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<AppUsuario>();

            modelBuilder.Entity<AppUsuario>().HasData(
                new AppUsuario
                {
                    Id = "c979437c-d850-47ef-bcbb-167b2524aban",
                    Active = true,
                    FirstName = "Admin",
                    LastName = "",
                    Name = "Admin test",
                    Email = "Admin@test.com",
                    EmailConfirmed = true,
                    UserName = "Admin@test.com",
                    NormalizedUserName = "Admin@test.com",
                    PasswordHash = hasher.HashPassword(null, "Testing123*"),
                    NormalizedEmail = "Admin@test.com"
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    RoleId = "97ac321b-af81-41f8-ab63-f71ac848ea7c",
                    UserId = "c979437c-d850-47ef-bcbb-167b2524aban"
                }
            );
        }
    }
}