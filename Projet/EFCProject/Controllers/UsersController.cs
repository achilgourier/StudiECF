using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCProject.Data;
using EFCProject.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;

namespace EFCProject.Controllers
{
    public class UserController : Controller
    {


        private readonly EFCProject.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(ApplicationDbContext context, 
								UserManager<ApplicationUser> userManager ,
								RoleManager<IdentityRole> roleManager ,
								SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;


        }

        
		[HttpGet]
        public async Task<IActionResult> ListUsers(string roleTri)
        {
			var users = await _userManager.Users.ToListAsync();

			if (string.IsNullOrEmpty(roleTri))
			{
				// Si roleTri est null ou vide, afficher tous les utilisateurs
				users = await _userManager.Users.ToListAsync();
			}
			else
			{
				// Si roleTri est spécifié, filtrer les utilisateurs par rôle
				var usersInRole = await _userManager.GetUsersInRoleAsync(roleTri);
				users = usersInRole.ToList();
			}



			List<UserViewModel> model = new List<UserViewModel>();

            foreach (ApplicationUser user in users)
            {
                UserViewModel userViewModel = new UserViewModel
                {
                    User = user,
                    Roles = await _userManager.GetRolesAsync(user)


                };

                model.Add(userViewModel);


			}
			return View(model); 
        }
		
		[HttpPost]
		public async Task<IActionResult> EditUsersInRole(string roleName, string userId )
        {   
            
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
				return View("NotFound");
			}

			if (!await _roleManager.RoleExistsAsync(roleName))
			{
				
				// Si le rôle n'existe pas, vous pouvez le créer ici
				var role = new IdentityRole(roleName);
				await _roleManager.CreateAsync(role);
				return View("NotFound");
			}

			var userRoles = await _userManager.GetRolesAsync(user);
			await _userManager.RemoveFromRolesAsync(user, userRoles);
			// Ajoutez l'utilisateur au rôle
			await _userManager.AddToRoleAsync(user, roleName);

			return Json(new { success = true, role = roleName });
		}


		public async Task<IActionResult> EditPasword(string userId, string currentPassword, string newPassword)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound($"ID introuvable : '{_userManager.GetUserId(User)}'.");
			}
			var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
			if (changePasswordResult.Succeeded)
			{
				await _signInManager.RefreshSignInAsync(user);
				return View("ListUsers");
			}
			else
			{
				foreach (var error in changePasswordResult.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
                return View("ListUsers");
            }
			
		}


	}
}
