using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CNPMNC.ViewModels;
using CNPMNC.Models;

namespace CNPMNC.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AccountController(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager,
			RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(
					model.Username,
					model.Password,
					isPersistent: false,
					lockoutOnFailure: false
				);

				if (result.Succeeded)
				{
					var user = await _userManager.FindByNameAsync(model.Username);

					if (await _userManager.IsInRoleAsync(user, "Admin"))
					{
						return RedirectToAction("Dashboard", "Admin");
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}

				ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng!");
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new IdentityUser
				{
					UserName = model.Username,
					Email = model.Email
				};

				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					if (!await _roleManager.RoleExistsAsync("User"))
					{
						await _roleManager.CreateAsync(new IdentityRole("User"));
					}

					await _userManager.AddToRoleAsync(user, "User");
					await _signInManager.SignInAsync(user, isPersistent: false);

					TempData["Success"] = "Đăng ký thành công!";
					return RedirectToAction("Index", "Home");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		public IActionResult AccountPage()
		{
			var account = new UserAccount
			{
				FullName = "Nguyễn Văn A",
				Email = "nguyenvana@example.com",
				Phone = "0912345678",
				Passport = "C1234567",
				Address = "Hà Nội, Việt Nam",
				Tickets = new List<Ticket>
				{
					new Ticket { Id = 1, FlightCode = "VN123", From = "Hà Nội", To = "TP.HCM", DepartureTime = DateTime.Now.AddDays(-10), Price = 1500000 },
					new Ticket { Id = 2, FlightCode = "VJ456", From = "TP.HCM", To = "Đà Nẵng", DepartureTime = DateTime.Now.AddDays(-3), Price = 1200000 }
				}
			};

			return View(account);
		}
	}
}