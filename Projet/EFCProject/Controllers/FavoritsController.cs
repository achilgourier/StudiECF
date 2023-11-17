using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCProject.Data;
using EFCProject.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace EFCProject.Controllers
{
    public class FavoritsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GamesController> _logger;

        public FavoritsController(ApplicationDbContext context, ILogger<GamesController> logger )
        {
            _context = context;
            _logger = logger;

        }

		// GET: Favorits
		[Authorize]
		public async Task<IActionResult> Index()
        {
              return _context.Favorit != null ? 
                          View(await _context.Favorit.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Favorit'  is null.");
        }

        // GET: Favorits/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details()
        {

            //string userID = Membership.GetUser().ProviderUserKey.ToString();
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var userIdValue = userIdClaim.Value;
    

                    return Json(new { success = true, userId = userIdValue });
                }
            }



            return Json(new { success = false, userId = "non" });
        }

        
        [HttpPost]
        public async Task<IActionResult> AddFav(int gameId)
        {
            Favorit favorit = new Favorit();
            
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    favorit.UserId = userIdClaim.Value;
                    favorit.GameId = gameId;
                    if (ModelState.IsValid)
                    {
                        _context.Add(favorit);
                        _context.Game.Find(gameId).Score++;
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, score = _context.Game.Find(gameId).Score });
                    }

                    
                }
		    }
            return View("../Account/Register");
        }
        [HttpDelete]
        public async Task<IActionResult> RemFav(int gameId)
        {

            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var supprFav = _context.Favorit.FirstOrDefault(f => f.GameId == gameId && f.UserId == userIdClaim.Value);
                    _context.Favorit.Remove(supprFav);    
                    _context.Game.Find(gameId).Score--;
                    await _context.SaveChangesAsync();

  

                    return Json(new { success = true , score = _context.Game.Find(gameId).Score });
                }
            }
            return View("../../Account/Register");
        }

		/*

        // GET: Favorits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Favorit == null)
            {
                return NotFound();
            }

            var favorit = await _context.Favorit.FindAsync(id);
            if (favorit == null)
            {
                return NotFound();
            }
            return View(favorit);
        }

        // POST: Favorits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,GameId")] Favorit favorit)
        {
            if (id != favorit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favorit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoritExists(favorit.Id))
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
            return View(favorit);
        }

        // GET: Favorits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Favorit == null)
            {
                return NotFound();
            }

            var favorit = await _context.Favorit
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favorit == null)
            {
                return NotFound();
            }

            return View(favorit);
        }

        // POST: Favorits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Favorit == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Favorit'  is null.");
            }
            var favorit = await _context.Favorit.FindAsync(id);
            if (favorit != null)
            {
                _context.Favorit.Remove(favorit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoritExists(int id)
        {
          return (_context.Favorit?.Any(e => e.Id == id)).GetValueOrDefault();
        }*/
	}
}
