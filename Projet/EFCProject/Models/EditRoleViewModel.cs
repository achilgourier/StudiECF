using Microsoft.Build.Framework;

namespace EFCProject.Models
{
	public class EditRoleViewModel
	{
		public string Id { get; set; }
		[Required]
		public string RoleName { get; set; }

		public List<string> User { get; set; }
		
	}
}
