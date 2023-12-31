﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCProject.Data;
using EFCProject.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace EFCProject.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Messages
        [Authorize(Roles = "CommManager")]
        public async Task<IActionResult> Index()
        {
              return _context.Message != null ? 
                          View(await _context.Message.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Message'  is null.");
        }

        // GET: Messages/Details/5
        [Authorize(Roles = "CommManager")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            return View();
        }

		public IActionResult Display()
		{
			// Récupérer les 5 derniers messages (exemple avec Entity Framework Core)
			var latestMessages = _context.Message
				.OrderByDescending(m => m.Date) // Assurez-vous d'ajuster selon votre modèle de données
				.Take(3)
				.ToList();

			// Retourner les messages au format JSON
			return Json(latestMessages);
		}

		[Authorize(Roles = "CommManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,UserName,Date")] Message message)
        {

            message.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [Authorize(Roles = "CommManager")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Message == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Message'  is null.");
            }
            var message = await _context.Message.FindAsync(id);
            if (message != null)
            {
                _context.Message.Remove(message);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
          return (_context.Message?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
