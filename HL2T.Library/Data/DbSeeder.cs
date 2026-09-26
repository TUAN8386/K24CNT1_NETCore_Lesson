using HL2T.Library.Models;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Data;

/// <summary>
/// Khởi tạo dữ liệu ban đầu.
/// - Nếu CSDL chưa có dữ liệu (chạy bằng EnsureCreated) thì thêm vai trò, tài khoản và sách mẫu.
/// - Nếu CSDL được tạo bằng script SQL, mật khẩu mẫu có dạng "SEED:&lt;mật khẩu&gt;" sẽ được băm lại bằng BCrypt.
/// </summary>
public static class DbSeeder
{
    public static void Seed(ThuVienDbContext db)
    {
        if (!db.VaiTros.Any())
        {
            db.VaiTros.AddRange(
                new VaiTro { TenVaiTro = VaiTroNames.Admin, MoTa = "Quản trị viên hệ thống" },
                new VaiTro { TenVaiTro = VaiTroNames.ThuThu, MoTa = "Thủ thư – quản lý kho sách và mượn trả" },
                new VaiTro { TenVaiTro = VaiTroNames.DocGia, MoTa = "Độc giả – tra cứu và mượn sách" });
            db.SaveChanges();
        }

        if (!db.NguoiDungs.Any())
        {
            int Role(string ten) => db.VaiTros.Single(v => v.TenVaiTro == ten).MaVaiTro;
            db.NguoiDungs.AddRange(
                new NguoiDung { TenDangNhap = "admin", MatKhauHash = "SEED:Admin@123", HoTen = "Nguyễn Văn Hùng", Email = "admin@hl2t.edu.vn", MaVaiTro = Role(VaiTroNames.Admin) },
                new NguoiDung { TenDangNhap = "thuthu", MatKhauHash = "SEED:ThuThu@123", HoTen = "Trần Thu Hà", Email = "thuthu@hl2t.edu.vn", MaVaiTro = Role(VaiTroNames.ThuThu) },
                new NguoiDung { TenDangNhap = "docgia", MatKhauHash = "SEED:DocGia@123", HoTen = "Nguyễn Minh Anh", Email = "docgia@hl2t.edu.vn", MaVaiTro = Role(VaiTroNames.DocGia) });
            db.SaveChanges();
        }

        if (!db.DanhMucSachs.Any())
        {
            var cntt = new DanhMucSach { TenDanhMuc = "Công nghệ thông tin" };
            var kt = new DanhMucSach { TenDanhMuc = "Kinh tế" };
            var kn = new DanhMucSach { TenDanhMuc = "Kỹ năng sống" };
            db.DanhMucSachs.AddRange(cntt, kt, kn);
            db.Sachs.AddRange(
                new Sach { TenSach = "Lập trình C# từ cơ bản đến nâng cao", TacGia = "Phạm Công Ngô", NamXuatBan = 2024, GiaBia = 185000, TongSoLuong = 5, SoLuongKhaDung = 5, DanhMucSach = cntt },
                new Sach { TenSach = "ASP.NET Core MVC thực chiến", TacGia = "Nguyễn Văn Hiếu", NamXuatBan = 2023, GiaBia = 210000, TongSoLuong = 4, SoLuongKhaDung = 4, DanhMucSach = cntt },
                new Sach { TenSach = "Kinh tế vĩ mô căn bản", TacGia = "Hoàng Văn Nam", NamXuatBan = 2022, GiaBia = 99000, TongSoLuong = 3, SoLuongKhaDung = 3, DanhMucSach = kt },
                new Sach { TenSach = "Kỹ năng giao tiếp hiệu quả", TacGia = "Vũ Thu Trang", NamXuatBan = 2023, GiaBia = 89000, TongSoLuong = 7, SoLuongKhaDung = 7, DanhMucSach = kn });
            db.SaveChanges();
        }

        // Băm các mật khẩu mẫu dạng "SEED:..."
        var canBam = db.NguoiDungs.Where(n => n.MatKhauHash.StartsWith("SEED:")).ToList();
        foreach (var nd in canBam)
            nd.MatKhauHash = BCrypt.Net.BCrypt.HashPassword(nd.MatKhauHash.Substring(5));
        if (canBam.Count > 0) db.SaveChanges();
    }
}
