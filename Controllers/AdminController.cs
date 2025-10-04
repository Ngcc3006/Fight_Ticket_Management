using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CNPMNC.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AdminController(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Dashboard()
		{
			var users = _userManager.Users.ToList();
			var userList = new List<UserWithRoleViewModel>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				userList.Add(new UserWithRoleViewModel
				{
					Id = user.Id,
					UserName = user.UserName ?? "",
					Email = user.Email ?? "",
					Roles = string.Join(", ", roles)
				});
			}

			return View(userList);
		}

		[AllowAnonymous]
		public async Task<IActionResult> CreateFirstAdmin()
		{
			if (await _roleManager.RoleExistsAsync("Admin"))
			{
				return Content("Admin role đã tồn tại! Username: admin, Password: Admin@123");
			}

			await _roleManager.CreateAsync(new IdentityRole("Admin"));
			await _roleManager.CreateAsync(new IdentityRole("User"));

			var adminUser = new IdentityUser
			{
				UserName = "admin",
				Email = "admin@example.com"
			};

			var result = await _userManager.CreateAsync(adminUser, "Admin@123");

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(adminUser, "Admin");
				return Content("Tạo Admin thành công! Username: admin, Password: Admin@123");
			}

			return Content("Lỗi: " + string.Join(", ", result.Errors.Select(e => e.Description)));
		}

		[HttpPost]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				await _userManager.DeleteAsync(user);
				TempData["Success"] = "Xóa người dùng thành công!";
			}
			return RedirectToAction("Dashboard");
		}

		[HttpPost]
		public async Task<IActionResult> ChangeRole(string userId, string role)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				var currentRoles = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, currentRoles);
				await _userManager.AddToRoleAsync(user, role);
				TempData["Success"] = "Thay đổi quyền thành công!";
			}
			return RedirectToAction("Dashboard");
		}
	}

	public class UserWithRoleViewModel
	{
		public string Id { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Roles { get; set; } = string.Empty;
	}
}