using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CNPMNC.Controllers  // ✅ Thêm namespace
{
	[Authorize]
	public class ProfileController : Controller
	{
		public IActionResult Index()
		{
			var userName = User.Identity?.Name ?? "Guest";  // ✅ Thêm ?? "Guest"
			return Content($"Xin chào {userName}, bạn đã đăng nhập thành công!");
		}
	}
}