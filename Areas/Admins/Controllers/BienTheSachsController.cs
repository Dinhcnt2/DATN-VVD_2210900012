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
    public class BienTheSachsController : Controller
    {
        private readonly BookstoreContext _context;

        public BienTheSachsController(BookstoreContext context)
        {
            _context = context;
        }

        // GET: Admins/BienTheSachs
        public async Task<IActionResult> Index()
        {
            var bookstoreContext = _context.BienTheSaches.Include(b => b.LoaiBia).Include(b => b.NgonN).Include(b => b.SanPham);
            return View(await bookstoreContext.ToListAsync());
        }

        // GET: Admins/BienTheSachs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bienTheSach = await _context.BienTheSaches
                .Include(b => b.LoaiBia)
                .Include(b => b.NgonN)
                .Include(b => b.SanPham)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bienTheSach == null)
            {
                return NotFound();
            }

            return View(bienTheSach);
        }

        // GET: Admins/BienTheSachs/Create
        public IActionResult Create()
        {
            ViewData["LoaiBiaId"] = new SelectList(_context.LoaiBia, "MaLoaiBia", "MaLoaiBia");
            ViewData["NgonNguId"] = new SelectList(_context.NgonNgus, "MaNgonNgu", "MaNgonNgu");
            ViewData["SanPhamId"] = new SelectList(_context.SanPhams, "Id", "Id");
            return View();
        }

        // POST: Admins/BienTheSachs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SanPhamId,LoaiBiaId,NgonNguId,SoLuongTon")] BienTheSach bienTheSach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bienTheSach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiBiaId"] = new SelectList(_context.LoaiBia, "MaLoaiBia", "MaLoaiBia", bienTheSach.LoaiBiaId);
            ViewData["NgonNguId"] = new SelectList(_context.NgonNgus, "MaNgonNgu", "MaNgonNgu", bienTheSach.NgonNguId);
            ViewData["SanPhamId"] = new SelectList(_context.SanPhams, "Id", "Id", bienTheSach.SanPhamId);
            return View(bienTheSach);
        }

        // GET: Admins/BienTheSachs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bienTheSach = await _context.BienTheSaches.FindAsync(id);
            if (bienTheSach == null)
            {
                return NotFound();
            }
            ViewData["LoaiBiaId"] = new SelectList(_context.LoaiBia, "MaLoaiBia", "MaLoaiBia", bienTheSach.LoaiBiaId);
            ViewData["NgonNguId"] = new SelectList(_context.NgonNgus, "MaNgonNgu", "MaNgonNgu", bienTheSach.NgonNguId);
            ViewData["SanPhamId"] = new SelectList(_context.SanPhams, "Id", "Id", bienTheSach.SanPhamId);
            return View(bienTheSach);
        }

        // POST: Admins/BienTheSachs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SanPhamId,LoaiBiaId,NgonNguId,SoLuongTon")] BienTheSach bienTheSach)
        {
            if (id != bienTheSach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bienTheSach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BienTheSachExists(bienTheSach.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiBiaId"] = new SelectList(_context.LoaiBia, "MaLoaiBia", "MaLoaiBia", bienTheSach.LoaiBiaId);
            ViewData["NgonNguId"] = new SelectList(_context.NgonNgus, "MaNgonNgu", "MaNgonNgu", bienTheSach.NgonNguId);
            ViewData["SanPhamId"] = new SelectList(_context.SanPhams, "Id", "Id", bienTheSach.SanPhamId);
            return View(bienTheSach);
        }

        // GET: Admins/BienTheSachs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bienTheSach = await _context.BienTheSaches
                .Include(b => b.LoaiBia)
                .Include(b => b.NgonN)
                .Include(b => b.SanPham)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bienTheSach == null)
            {
                return NotFound();
            }

            return View(bienTheSach);
        }

        // POST: Admins/BienTheSachs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bienTheSach = await _context.BienTheSaches.FindAsync(id);
            if (bienTheSach != null)
            {
                _context.BienTheSaches.Remove(bienTheSach);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BienTheSachExists(int id)
        {
            return _context.BienTheSaches.Any(e => e.Id == id);
        }
    }
}
