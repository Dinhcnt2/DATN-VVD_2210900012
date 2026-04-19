using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VVD_2210900012_DATN.Models;

namespace VVD_2210900012_DATN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookstoreContext _context;

        public HomeController(ILogger<HomeController> logger, BookstoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // lấy 8 sách mới nhất
            var sach = _context.SanPhams
                        .Include(x => x.DanhMuc)
                        .Take(8)
                        .ToList();

            return View(sach);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}