using EFCProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EFCProject.Controllers
{
    
    
    public class HomeController : Controller
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Asset/Images/CarouselImage");
            string folderPathFromRoot = "~/Asset/Images/CarouselImage";
            // Vérifie si le dossier existe
            if (Directory.Exists(folderPath))
            {
                // Obtient la liste des fichiers dans le dossier
                string[] fileNames = Directory.GetFiles(folderPath);

                // Crée une liste d'ImageModel en utilisant les noms de fichiers
                var images = new List<ImageModel>();
                foreach (var fileName in fileNames)
                {
                    images.Add(new ImageModel
                    {
                        ImagePath = $"{folderPathFromRoot}/{Path.GetFileName(fileName)}",
                        Description = $"Description de {Path.GetFileNameWithoutExtension(fileName)}"
                    });
                }

                // Passe la liste d'images à la vue
                
                return View("Index", images);
            }
            else
            {
                return Problem("Le dossier", folderPath);
            }
        }

        
        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}