using Dal.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Entities.Settings;
using System.Reflection;

namespace Dal
{
    public class ApplicationDbContext : IdentityDbContext<AppUsuario, Role, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(AppUserConfig)));

            SeedingRoles.Seed(builder);
            SeedingAdministrators.Seed(builder);
        }
    }
}