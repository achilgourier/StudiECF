namespace EFCProject.Models
{

	public class UserViewModel
	{
		public ApplicationUser User { get; set; }
		public IList<string> Roles { get; set; }
	}
}
