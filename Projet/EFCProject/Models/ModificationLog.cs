using System.ComponentModel.DataAnnotations;

namespace EFCProject.Models
{
    public class ModificationLog
<<<<<<< Updated upstream
    {   
        [Key]
        public int Id { get; set; }

        public int GameId { get; set; }
        public float Budget { get; set; }

        public DateTime DateChanged { get; set; }


        public string Commentaire { get; set; }
      
=======
    {

            [Key]
            public int Id { get; set; }
            public int GameId { get; set; }
            public float Budget { get; set; }
            public DateTime dateModiff { get; set; }
            public string Commentaire { get; set; }

>>>>>>> Stashed changes
    }
}
