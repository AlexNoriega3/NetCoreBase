using Microsoft.AspNetCore.Identity;

namespace Models.Entities
{
    public class UserRole : IdentityUserRole<string>
    {
        public AppUsuario User { get; set; }
        public Role Role { get; set; }
    }
}