using HL2T.Library.Data;
using HL2T.Library.Models;
using HL2T.Library.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Services;

public interface ISachService
{
    Task<TimKiemSachVM> TimKiemAsync(TimKiemSachVM dieuKien);
    Task<Sach?> LayTheoMaAsync(int maSach);
    Task<KetQua> ThemAsync(SachFormVM vm);
    Task<KetQua> CapNhatAsync(SachFormVM vm);
    Task<KetQua> XoaAsync(int maSach);
}

public class SachService : ISachService
{
    private readonly ThuVienDbContext _db;
    private readonly IWebHostEnvironment _env;

    public SachService(ThuVienDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task<TimKiemSachVM> TimKiemAsync(TimKiemSachVM dk)
    {
        var q = _db.Sachs.Include(s => s.DanhMucSach).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(dk.TuKhoa))
        {
            var k = dk.TuKhoa.Trim();
            q = q.Where(s => s.TenSach.Contains(k) || s.TacGia.Contains(k) || (s.ISBN != null && s.ISBN.Contains(k)));
        }
        if (dk.MaDanhMuc.HasValue) q = q.Where(s => s.MaDanhMuc == dk.MaDanhMuc);
        if (dk.ChiConSach) q = q.Where(s => s.SoLuongKhaDung > 0);

        dk.Trang = Math.Max(1, dk.Trang);
        dk.TongSo = await q.CountAsync();
        dk.KetQua = await q.OrderByDescending(s => s.MaSach)
            .Skip((dk.Trang - 1) * dk.KichThuocTrang).Take(dk.KichThuocTrang).ToListAsync();
        dk.DanhMucs = await _db.DanhMucSachs.AsNoTracking().OrderBy(d => d.TenDanhMuc).ToListAsync();
        return dk;
    }

    public Task<Sach?> LayTheoMaAsync(int maSach) =>
        _db.Sachs.Include(s => s.DanhMucSach).FirstOrDefaultAsync(s => s.MaSach == maSach);

    public async Task<KetQua> ThemAsync(SachFormVM vm)
    {
        if (!string.IsNullOrWhiteSpace(vm.ISBN) && await _db.Sachs.AnyAsync(s => s.ISBN == vm.ISBN))
            return KetQua.Loi("ISBN đã tồn tại trong hệ thống.");

        var sach = new Sach
        {
            ISBN = string.IsNullOrWhiteSpace(vm.ISBN) ? null : vm.ISBN.Trim(),
            TenSach = vm.TenSach.Trim(),
            TacGia = vm.TacGia.Trim(),
            NhaXuatBan = vm.NhaXuatBan,
            NamXuatBan = vm.NamXuatBan,
            GiaBia = vm.GiaBia,
            MoTa = vm.MoTa,
            TongSoLuong = vm.TongSoLuong,
            SoLuongKhaDung = vm.TongSoLuong,   // khởi tạo khả dụng = tổng
            MaDanhMuc = vm.MaDanhMuc,
            AnhBia = await LuuAnhAsync(vm.AnhBia)
        };
        _db.Sachs.Add(sach);
        await _db.SaveChangesAsync();
        return KetQua.Ok("Thêm sách thành công.", sach.MaSach);
    }

    public async Task<KetQua> CapNhatAsync(SachFormVM vm)
    {
        var sach = await _db.Sachs.FindAsync(vm.MaSach);
        if (sach == null) return KetQua.Loi("Không tìm thấy sách.");
        if (!string.IsNullOrWhiteSpace(vm.ISBN) && await _db.Sachs.AnyAsync(s => s.ISBN == vm.ISBN && s.MaSach != vm.MaSach))
            return KetQua.Loi("ISBN đã tồn tại trong hệ thống.");

        int dangMuon = sach.TongSoLuong - sach.SoLuongKhaDung;
        if (vm.TongSoLuong < dangMuon)
            return KetQua.Loi($"Tổng số lượng không được nhỏ hơn số sách đang được mượn ({dangMuon}).");

        sach.ISBN = string.IsNullOrWhiteSpace(vm.ISBN) ? null : vm.ISBN.Trim();
        sach.TenSach = vm.TenSach.Trim();
        sach.TacGia = vm.TacGia.Trim();
        sach.NhaXuatBan = vm.NhaXuatBan;
        sach.NamXuatBan = vm.NamXuatBan;
        sach.GiaBia = vm.GiaBia;
        sach.MoTa = vm.MoTa;
        sach.MaDanhMuc = vm.MaDanhMuc;
        sach.TongSoLuong = vm.TongSoLuong;
        sach.SoLuongKhaDung = vm.TongSoLuong - dangMuon;
        var anh = await LuuAnhAsync(vm.AnhBia);
        if (anh != null) sach.AnhBia = anh;

        await _db.SaveChangesAsync();
        return KetQua.Ok("Cập nhật sách thành công.");
    }

    public async Task<KetQua> XoaAsync(int maSach)
    {
        var sach = await _db.Sachs.FindAsync(maSach);
        if (sach == null) return KetQua.Loi("Không tìm thấy sách.");
        // QĐ6: không xóa sách đang có trong phiếu mượn còn hiệu lực
        bool dangMuon = await _db.ChiTietPhieuMuons.AnyAsync(c => c.MaSach == maSach && c.NgayTraThucTe == null
            && c.PhieuMuon!.TrangThai != TrangThaiPhieu.DaHuy && c.PhieuMuon.TrangThai != TrangThaiPhieu.DaTra);
        if (dangMuon) return KetQua.Loi("Sách đang được mượn hoặc chờ duyệt, không thể xóa.");
        if (await _db.ChiTietPhieuMuons.AnyAsync(c => c.MaSach == maSach))
            return KetQua.Loi("Sách đã có lịch sử mượn trả, không thể xóa (chỉ có thể cập nhật số lượng về 0).");

        _db.Sachs.Remove(sach);
        await _db.SaveChangesAsync();
        return KetQua.Ok("Đã xóa sách.");
    }

    private async Task<string?> LuuAnhAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0) return null;
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (ext is not (".jpg" or ".jpeg" or ".png" or ".webp") || file.Length > 2 * 1024 * 1024)
            return null;
        var folder = Path.Combine(_env.WebRootPath, "images", "sach");
        Directory.CreateDirectory(folder);
        var name = $"{Guid.NewGuid():N}{ext}";
        await using var fs = new FileStream(Path.Combine(folder, name), FileMode.Create);
        await file.CopyToAsync(fs);
        return "/images/sach/" + name;
    }
}
