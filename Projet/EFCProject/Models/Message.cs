using System.ComponentModel.DataAnnotations;

namespace EFCProject.Models
{
	public class Message
	{
		[Key]
		public int Id { get; set; }
		public string Text { get; set; }
		public string UserName { get; set; }
		public DateTime Date { get; set; }
		

	}
}
