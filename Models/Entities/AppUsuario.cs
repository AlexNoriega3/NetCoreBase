using Microsoft.AspNetCore.Identity;
using Models.Enums;
using Models.Globals;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class AppUsuario : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Sortable(OrderBy = "Name")]
        public string Name { get; set; }

        [Sortable(OrderBy = "Email")]
        public override string Email { get; set; }

        public string Url { get; set; }
        public string TitleAbbreviation { get; set; }
        public string Image { get; set; }
        public GenderEnum Gender { get; set; }
        public bool Active { get; set; }
        public string? ParentId { get; set; }
        public string ProjectId { get; set; }

        [ForeignKey("ParentId")]
        public AppUsuario Parent { get; set; }

        public List<AppUsuario> Subordinates { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName + " " + Name;
        }
    }
}