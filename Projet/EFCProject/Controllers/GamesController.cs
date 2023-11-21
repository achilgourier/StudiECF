using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCProject.Data;
using EFCProject.Models;
using Microsoft.AspNetCore.Authorization;
using static System.Formats.Asn1.AsnWriter;
using System.Reflection;
using System.IO;
using System.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Security.Claims;

namespace EFCProject.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GamesController> _logger;
        private readonly IWebHostEnvironment _environment;


        public GamesController(ApplicationDbContext context, ILogger<GamesController> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

		// GET: Games

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Index()
        {
            return _context.Game != null ?
                        View(await _context.Game.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Game'  is null.");
        }
		[Authorize(Roles = "Producer")]
		public async Task<IActionResult> IndexProd()
		{
			return _context.Game != null ?
						View(await _context.Game.ToListAsync()) :
						Problem("Entity set 'ApplicationDbContext.Game'  is null.");
		}

		// GET: Games/ShowSearchForm
		public async Task<IActionResult> ShowSearchForm(string origine)
        {   
           
            
            return _context.Game != null ?
                                View(await _context.Game.ToListAsync()) :
                                Problem("Entity set 'ApplicationDbContext.Game'  is null.");
        }
        //POST : Games/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchInput, string origine)
        {
            if (origine == null)
            {
                origine = "ShowSearchForm";
            }

            Type type = typeof(Game);
            if (SearchInput != null)
            {
                // Obtenez les propriétés de l'objet Game
                PropertyInfo[] properties = type.GetProperties();
                List<Game> tabGame = await _context.Game.ToListAsync();
                List<Game> vide = new List<Game>();
                foreach (Game game in tabGame)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        Type propertyType = property.PropertyType;

                        if (propertyType == typeof(string))
                        {
                            string propertyValue = (string)property.GetValue(game);
                            if (propertyValue != null && propertyValue.Contains(SearchInput))
                            {
                                vide.Add(game);
                            }

                        }
                    }
                }
                return View(origine, vide);
            }
            return View(origine, await _context.Game.ToListAsync());

        }
        public async Task<IActionResult> ShowSearchResultsProd(string SearchInput)
        {

            Type type = typeof(Game);
            if (SearchInput != null)
            {
                // Obtenez les propriétés de l'objet Game
                PropertyInfo[] properties = type.GetProperties();
                List<Game> tabGame = await _context.Game.ToListAsync();
                List<Game> vide = new List<Game>();
                foreach (Game game in tabGame)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        Type propertyType = property.PropertyType;
                        if (propertyType == typeof(string))
                        {
                            string propertyValue = (string)property.GetValue(game);
                            if (propertyValue != null && propertyValue.Contains(SearchInput))
                            {
                                vide.Add(game);
                            }

                        }
                    }
                }
                return View("IndexProd", vide);
            }
            return View("IndexProd", await _context.Game.ToListAsync());
        }



        [Authorize]
		public async Task<IActionResult> Displayfav()
        {


            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                

                // Effectuer une jointure entre les tables User et FavoriteGame
                var query = from game in _context.Game
                            join favorite in _context.Favorit on game.Id equals favorite.GameId
                            where favorite.UserId == userIdClaim.Value
                            select game;

                // Exécutez la requête et récupérez les résultats
                var favoriteGames = query.ToList();

                return favoriteGames != null ?
                    View("Displayfav", favoriteGames) :
                    Problem("Entity set 'ApplicationDbContext.Game'  is null.");
            }

            return View("../Account/Register");


        }

        
        [Authorize]
        [HttpPost]
        public IActionResult AddFav(int gameId, int userID ,string origin)
        {
            // Récupérez le jeu à partir de la base de données
            var game = _context.Game.Find(gameId);

            
            // Incrémentez le Score
             game.Score++;

            // Enregistrez les modifications dans la base de données
            _context.SaveChanges();

            // Vous pouvez renvoyer quelque chose en réponse si nécessaire
            return Json(new { success = true, newScore = game.Score });
        }


        public async Task<IActionResult> DisplayGamesInTab()
        {
            return _context.Game != null ?
                          View(await _context.Game.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Game'  is null.");
        }


        public async Task<IActionResult> DisplayGamesSorted(string sortBy, string origine)
        {
            
            if(origine == null)
            {
                origine = "ShowSearchForm";
            }
            Type type = typeof(Game);

            // Obtenez les propriétés de l'objet Game
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == sortBy)
                {

                    List<Game> tabGame = await _context.Game.ToListAsync();
                    //List<Game> SortedList = tabGame.OrderBy(o => property.GetValue(o, null)).ToList();
					List<Game> SortedList = tabGame.OrderByDescending(o => property.GetValue(o, null)).ToList();
                    
					return View(origine, SortedList);
  
                }
            }
            return View(origine, await _context.Game.ToListAsync());
        }




        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
                        
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            int isFavorite = 0;
            // Obtenez l'ID de l'utilisateur actuel (assurez-vous que votre application gère correctement l'authentification)
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if(userIdClaim != null)
                {
                    // Vérifiez si le jeu est déjà en favori pour cet utilisateur
                    if(_context.Favorit.Any(f => f.UserId == userIdClaim.Value && f.GameId == id))
                    {
                       isFavorite = 1;

					}

				}
                
            }
            var viewModel = new GameViewModel
            {
                Game = game,
                IsInFavorites = isFavorite
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Studio,Support,Size,Score,Engine,CreateDate,EndDate,Budget,Statut,Type,Image")] Game game, IFormFile postedFile)
        {
            game.Score = 0;
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    string path = _environment.WebRootPath + "/Asset/Images/Games/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = postedFile.FileName;
                    using (var stream = new System.IO.FileStream(path + fileName, System.IO.FileMode.Create))
                    {
                        await postedFile.CopyToAsync(stream);
                    }
                    game.Image = fileName;
                }
                
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(game);
        }

        // GET: Games/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Description,Studio,Support,Size,Engine,CreateDate,EndDate,Budget,Statut,Type,Image")] Game game, IFormFile postedFile)
        {
            var existingGame = await _context.Game.FindAsync(id);


            if (existingGame == null)
            {
                return RedirectToAction("Index");
            }


            if (ModelState.IsValid)
            {
                    
                if (postedFile != null)
                {
                    string path = _environment.WebRootPath + "/Asset/Images/Games/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string imagePath = path + game.Image;
                    // Vérifie si le fichier existe avant de le supprimer
                    if (System.IO.File.Exists(imagePath))
                    {
                        // Supprime le fichier
                        System.IO.File.Delete(imagePath);
                        
                    }
                    using (var stream = new System.IO.FileStream(path + postedFile.FileName, System.IO.FileMode.Create))
                    {
                        await postedFile.CopyToAsync(stream);
                    }

                    existingGame.Description = game.Description;
                    existingGame.Studio = game.Studio;
                    existingGame.Type = game.Type;
                    existingGame.Statut = game.Statut;
                    existingGame.Budget = game.Budget;
                    existingGame.EndDate = game.EndDate;
                    existingGame.CreateDate = game.CreateDate;
                    existingGame.Engine = game.Engine;
                    existingGame.Size = game.Size;
                    existingGame.Support = game.Support;
                    existingGame.Image = postedFile.FileName;
                }

                _context.Update(existingGame);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(game);
        }



        [Authorize(Roles = "Producer")]
        public async Task<IActionResult> EditProd(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }
        
        [Authorize(Roles = "Producer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProd(int id, [Bind("EndDate,Budget,Statut")] Game updatedGame, string commentaire)
        {

            // Récupérer le jeu existant depuis la base de données
            var existingGame = await _context.Game.FindAsync(id);

            if (existingGame == null)
            {
                return NotFound();
            }
            // Mettre à jour les propriétés spécifiées uniquement

            existingGame.EndDate = updatedGame.EndDate;
            existingGame.Budget = updatedGame.Budget;
            existingGame.Statut = updatedGame.Statut;

            if (ModelState.IsValid)
            {
                // Mettre à jour le jeu existant
                _context.Update(existingGame);
                await _context.SaveChangesAsync();
                
                if (commentaire != null)
                {
                    ModificationLog modif = new ModificationLog();
                    modif.GameId = existingGame.Id;
                    modif.Budget = existingGame.Budget;
                    modif.DateChanged = DateTime.Now;
                    modif.Commentaire = commentaire;

                    _context.Add(modif);
                    await _context.SaveChangesAsync();
                }

                


                return RedirectToAction("IndexProd");
            }


            // Si le modèle n'est pas valide, retourner à la vue avec le modèle existant
            return View(existingGame);
        }




        // GET: Games/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Game'  is null.");
            }
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                if (game.Image != null) {
                    string path = _environment.WebRootPath + "/Asset/Images/Games/";
                    string filePath = path + game.Image;
                    // Vérifie si le fichier existe avant de le supprimer
                        if (System.IO.File.Exists(filePath))
                        {
                        // Supprime le fichier
                            System.IO.File.Delete(filePath);
                            Console.WriteLine("Le fichier a été supprimé avec succès.");
                        }
                }
                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return (_context.Game?.Any(e => e.Id == id)).GetValueOrDefault();
        }




    }
}
