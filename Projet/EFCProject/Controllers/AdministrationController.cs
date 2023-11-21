using EFCProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EFCProject.Controllers
{

	
	public class AdministrationController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		public AdministrationController(RoleManager<IdentityRole> roleManager)
		{

			_roleManager = roleManager;
		}

		[HttpGet]
		public IActionResult CreateRole()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateRole(CreateRoleViewModels model)
		{
			if (ModelState.IsValid)
			{
				IdentityRole identityRole = new IdentityRole
				{
					Name = model.RoleName
				};
				IdentityResult result = await _roleManager.CreateAsync(identityRole);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
			}
			
			return View();
		}
		[HttpGet]
		public IActionResult ListRoles()
		{
			var roles = _roleManager.Roles;
			return View(roles);
		}



	}
}
