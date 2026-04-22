using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VVD_2210900012_DATN.Models;

namespace VVD_2210900012_DATN.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class BienTheSachsController : Controller
    {
        private readonly BookstoreContext _context;

        public BienTheSachsController(BookstoreContext context)
        {
            _context = context;
        }

        // ===== INDEX =====
        public async Task<IActionResult> Index()
        {
            var data = _context.BienTheSaches
                .Include(x => x.SanPham)
                .Include(x => x.LoaiBia)
                .Include(x => x.NgonN); // navigation đúng

            return View(await data.ToListAsync());
        }

        // ===== CREATE GET =====
        public IActionResult Create()
        {
            LoadDropdown();
            return View();
        }

        // ===== CREATE POST =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BienTheSach model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdown(model);
                return View(model);
            }

            var exist = await _context.BienTheSaches.FirstOrDefaultAsync(x =>
                x.SanPhamId == model.SanPhamId &&
                x.LoaiBiaId == model.LoaiBiaId &&
                x.NgonNguId == model.NgonNguId
            );

            if (exist != null)
            {
                ModelState.AddModelError("", "❌ Biến thể đã tồn tại!");
                LoadDropdown(model);
                return View(model);
            }

            _context.BienTheSaches.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ===== LOAD DROPDOWN =====
        private void LoadDropdown(BienTheSach? model = null)
        {
            // 🔥 SÁCH
            ViewBag.SanPhamId = new SelectList(
                _context.SanPhams.ToList(),
                "Id",
                "TenSach",
                model?.SanPhamId
            );

            // 🔥 LOẠI BÌA
            ViewBag.LoaiBiaId = new SelectList(
                _context.LoaiBia.ToList(), // ✅ đúng
                "MaLoaiBia",
                "TenLoaiBia", // hiển thị đẹp hơn
                model?.LoaiBiaId
            );

            // 🔥 NGÔN NGỮ
            ViewBag.NgonNguId = new SelectList(
                _context.NgonNgus.ToList(), // ✅ đúng
                "MaNgonNgu",
                "TenNgonNgu",
                model?.NgonNguId
            );
        }
    }
}
