using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using VVD_2210900012_DATN.Models;

namespace VVD_2210900012_DATN.Controllers
{
    public class GioHangController : Controller
    {
        private readonly BookstoreContext _context;

        public GioHangController(BookstoreContext context)
        {
            _context = context;
        }

        // 🔥 CHẶN CHƯA LOGIN
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = HttpContext.Session.GetString("TenNguoiDung");

            if (user == null)
            {
                context.Result = new RedirectToActionResult("DangNhap", "TaiKhoan", null);
                return;
            }

            base.OnActionExecuting(context);
        }

        // ===== GET CART =====
        private List<GioHangItem> GetCart()
        {
            var session = HttpContext.Session.GetString("GioHang");

            if (!string.IsNullOrEmpty(session))
            {
                return JsonConvert.DeserializeObject<List<GioHangItem>>(session)
                       ?? new List<GioHangItem>();
            }

            return new List<GioHangItem>();
        }

        // ===== SAVE CART =====
        private void SaveCart(List<GioHangItem> cart)
        {
            HttpContext.Session.SetString("GioHang", JsonConvert.SerializeObject(cart));
        }

        // ===== MUA NGAY =====
        public IActionResult MuaNgay(int id)
        {
            var sach = _context.SanPhams.FirstOrDefault(x => x.Id == id);

            if (sach == null) return NotFound();

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                item.SoLuong++;
            }
            else
            {
                cart.Add(new GioHangItem
                {
                    Id = sach.Id,
                    TenSach = sach.TenSach ?? "",
                    Gia = sach.GiaSauGiam ?? sach.GiaGoc,
                    SoLuong = 1,
                    Anh = sach.AnhBia ?? ""
                });
            }

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // ===== HIỂN THỊ GIỎ =====
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // ===== UPDATE =====
        [HttpPost]
        public IActionResult Update(int id, int soluong)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.Id == id);

            if (item != null && soluong > 0)
            {
                item.SoLuong = soluong;
            }

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // ===== XOÁ =====
        public IActionResult Xoa(int id)
        {
            var cart = GetCart();

            cart.RemoveAll(x => x.Id == id);
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // ===== ĐẶT HÀNG (FIX FK CHUẨN) =====
        [HttpPost]
        public IActionResult DatHang(string ten, string sdt, string diachi)
        {
            var cart = GetCart();

            if (!cart.Any())
                return RedirectToAction("Index");

            decimal tongTien = cart.Sum(x => x.Gia * x.SoLuong);

            // tạo đơn hàng
            var don = new DonHang
            {
                MaDonHangCode = "DH" + DateTime.Now.Ticks,
                HoTen = ten,
                SoDienThoai = sdt,
                DiaChi = diachi,
                TongTien = tongTien,
                NgayDat = DateTime.Now
            };

            _context.DonHangs.Add(don);
            _context.SaveChanges();

            // 🔥 THÊM CHI TIẾT ĐƠN HÀNG (FIX FK)
            foreach (var item in cart)
            {
                var bienThe = _context.BienTheSaches
                    .FirstOrDefault(x => x.SanPhamId == item.Id);

                if (bienThe == null)
                {
                    // nếu chưa có biến thể → bỏ qua hoặc báo lỗi
                    continue;
                }

                var ct = new ChiTietDonHang
                {
                    MaDonHang = don.MaDonHang,
                    BienTheId = bienThe.Id,
                    SoLuong = item.SoLuong,
                    DonGia = item.Gia,
                    ThanhTien = item.Gia * item.SoLuong
                };

                _context.ChiTietDonHangs.Add(ct);
            }

            _context.SaveChanges();

            // xoá giỏ
            HttpContext.Session.Remove("GioHang");

            return RedirectToAction("ThanhToan", "DonHang", new { id = don.MaDonHang });
        }
    }

    // ===== MODEL GIỎ =====
    public class GioHangItem
    {
        public int Id { get; set; }
        public string TenSach { get; set; } = "";
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string Anh { get; set; } = "";
    }
}
