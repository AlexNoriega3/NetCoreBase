using Microsoft.AspNetCore.Identity;

namespace Models.Entities
{
    public class Role : IdentityRole
    {
        public bool Active { get; set; } = true;

        public string CreateBy { get; set; }

        public string UpdateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? DTUpdateDate { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}