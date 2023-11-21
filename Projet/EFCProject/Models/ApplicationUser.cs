

using Microsoft.AspNetCore.Identity;

namespace EFCProject.Models
{
	public class ApplicationUser: IdentityUser
	{
        public string? Pseudo { get; set; }


    }
}
