using Microsoft.AspNetCore.Mvc;
using VVD_2210900012_DATN.Models;
using System.Linq;

namespace VVD_2210900012_DATN.Controllers
{
    public class DonHangController : Controller
    {
        private readonly BookstoreContext _context;

        public DonHangController(BookstoreContext context)
        {
            _context = context;
        }

        // ===== TRANG THANH TOÁN QR =====
        public IActionResult ThanhToan(int id)
        {
            var don = _context.DonHangs.FirstOrDefault(x => x.MaDonHang == id);

            if (don == null)
                return NotFound();

            // ===== FIX NULL =====
            decimal tien = don.TongTien ?? 0;
            string maDon = don.MaDonHangCode ?? "DH" + don.MaDonHang;

            // ===== THÔNG TIN BANK =====
            string stk = "0976067728";
            string ten = "VUONG VAN DINH";
            string bank = "MB";

            // ===== QR CHUẨN =====
            string qrUrl = $"https://img.vietqr.io/image/{bank}-{stk}-compact.png?amount={tien}&addInfo={maDon}&accountName={ten}";

            ViewBag.QR = qrUrl;
            ViewBag.MaDon = maDon;
            ViewBag.TongTien = tien;

            return View();
        }
    }
}
