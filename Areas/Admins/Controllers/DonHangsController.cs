using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VVD_2210900012_DATN.Models;

namespace VVD_2210900012_DATN.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class DonHangsController : Controller
    {
        private readonly BookstoreContext _context;

        public DonHangsController(BookstoreContext context)
        {
            _context = context;
        }

        // ================= DANH SÁCH =================
        public async Task<IActionResult> Index()
        {
            var bookstoreContext = _context.DonHangs
                .Include(d => d.MaNguoiDungNavigation)
                .Include(d => d.Voucher);

            return View(await bookstoreContext.ToListAsync());
        }

        // ================= CHI TIẾT =================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var donHang = await _context.DonHangs
                .Include(d => d.MaNguoiDungNavigation)
                .Include(d => d.Voucher)
                .FirstOrDefaultAsync(m => m.MaDonHang == id);

            if (donHang == null) return NotFound();

            return View(donHang);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            ViewData["VoucherId"] = new SelectList(_context.Vouchers, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donHang);
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null) return NotFound();

            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DonHang donHang)
        {
            if (id != donHang.MaDonHang) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(donHang);
        }

        // ================= DELETE =================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var donHang = await _context.DonHangs
                .FirstOrDefaultAsync(m => m.MaDonHang == id);

            if (donHang == null) return NotFound();

            return View(donHang);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= 🔥 THÊM MỚI (QUAN TRỌNG) =================

        [HttpGet]
        public async Task<IActionResult> CapNhatThanhToan(int id, string status)
        {
            var don = await _context.DonHangs.FindAsync(id);
            if (don == null) return NotFound();

            don.TrangThaiThanhToan = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CapNhatTrangThai(int id, string status)
        {
            var don = await _context.DonHangs.FindAsync(id);
            if (don == null) return NotFound();

            don.TrangThai = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDonHang == id);
        }
    }
}
