using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Dal.Seeding
{
    public static class SeedingRoles
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            Role roleAdmin = new Role
            {
                Id = "97ac321b-af81-41f8-ab63-f71ac848ea7c",
                Name = "Administrator",
                NormalizedName = "ADMINISTRADOR",
                ConcurrencyStamp = "c90997ad-2a9a-4285-be34-86a907498275"
            };

            Role roleProvider = new Role
            {
                Id = "17baeb2c-bd36-48ad-b0cc-bc6077674ffa",
                Name = "Proveedor",
                NormalizedName = "PROVEEDOR",
                ConcurrencyStamp = "acbcadb6-1850-4ef8-8b2e-ae776d40fa23"
            };

            Role roleUser = new Role
            {
                Id = "21875d1b-f01a-4666-ba93-0f4aa35ff3b2",
                Name = "User",
                NormalizedName = "User",
                ConcurrencyStamp = "d7b5b004-8156-4939-8325-edfcf30c716e"
            };

            Role roleSubordinate = new Role
            {
                Id = "e5770656-e2c4-4e6a-b69b-ed915fa8e76d",
                Name = "Subordinate",
                NormalizedName = "SUBORDINATE",
                ConcurrencyStamp = "da7a4f42-ff3c-42a8-935a-62af68f978b0"
            };

            modelBuilder.Entity<Role>().HasData(roleAdmin, roleProvider, roleUser);
        }
    }
}