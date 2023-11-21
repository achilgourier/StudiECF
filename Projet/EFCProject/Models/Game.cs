using System.Drawing;
using System.ComponentModel.DataAnnotations;



namespace EFCProject.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Studio { get; set; }
        public string? Support { get; set; }
        public float Size { get; set; }    
        public int Score { get; set; }
        public string? Engine { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Budget { get; set; }
        public string? Statut { get; set; }
        public string? Type { get; set; }
        public string? Image { get; set; }

        public string? CreateurName { get; set; }

        public Game()
        {
            
        }

    }
}
