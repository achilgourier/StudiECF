using System.ComponentModel.DataAnnotations;

namespace EFCProject.Models
{
	public class CreateRoleViewModels
	{

		[Required]
		public string RoleName { get; set; }
	}
}
