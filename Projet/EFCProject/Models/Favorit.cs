using System.Drawing;
using System.ComponentModel.DataAnnotations;



namespace EFCProject.Models
{
    public class Favorit
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int GameId { get; set; }
        

    }
}
