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
        public async Task<IActionResult> Index()
        {
            return _context.Game != null ?
                        View(await _context.Game.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Game'  is nulaaaaaaaaaaaaaaaaaaaaaal.");
        }
        // GET: Games/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return _context.Game != null ?
                          View(await _context.Game.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Game'  is null.");
        }
        //POST : Games/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchTitle, String SearchSupport)
        {

            return View("ShowSearchForm", await _context.Game.Where(j => j.Title.Contains(SearchTitle) || j.Support.Contains(SearchSupport)).ToListAsync());
        }

        public async Task<IActionResult> DisplayGamesInTab()
        {
            return _context.Game != null ?
                          View(await _context.Game.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Game'  is null.");
        }
        public async Task<IActionResult> DisplayGamesSorted(string sortBy)
        {

            // Obtenez le type de l'objet Game
            Type type = typeof(Game);

            // Obtenez les propriétés de l'objet Game
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == sortBy)
                {

                    List<Game> tabGame = await _context.Game.ToListAsync();
                    List<Game> SortedList = tabGame.OrderBy(o => property.GetValue(o, null)).ToList();

                    return View("ShowSearchForm", SortedList);
                }
            }
            return View("ShowSearchForm", await _context.Game.ToListAsync());
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

            return View(game);
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

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Studio,Support,Size,Score,Engine,CreateDate,EndDate,Budget,Statut,Type,Image")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(game);
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
