using Microsoft.AspNetCore.Mvc;

namespace VVD_2210900012_DATN.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("VaiTro");

            if (string.IsNullOrEmpty(role) || role != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });
            }

            return View();
        }
    }
}