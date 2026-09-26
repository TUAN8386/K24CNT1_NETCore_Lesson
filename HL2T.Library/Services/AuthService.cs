using HL2T.Library.Data;
using HL2T.Library.Models;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Services;

public interface IAuthService
{
    Task<(NguoiDung? NguoiDung, string? Loi)> XacThucAsync(string tenDangNhap, string matKhau);
    Task<KetQua> DoiMatKhauAsync(int maNguoiDung, string matKhauCu, string matKhauMoi);
    string BamMatKhau(string matKhau);
}

public class AuthService : IAuthService
{
    private readonly ThuVienDbContext _db;
    public AuthService(ThuVienDbContext db) => _db = db;

    public async Task<(NguoiDung?, string?)> XacThucAsync(string tenDangNhap, string matKhau)
    {
        var nd = await _db.NguoiDungs.Include(x => x.VaiTro)
            .FirstOrDefaultAsync(x => x.TenDangNhap == tenDangNhap || x.Email == tenDangNhap);

        if (nd == null || !BCrypt.Net.BCrypt.Verify(matKhau, nd.MatKhauHash))
            return (null, "Tên đăng nhập hoặc mật khẩu không đúng.");
        if (!nd.TrangThai)
            return (null, "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
        return (nd, null);
    }

    public async Task<KetQua> DoiMatKhauAsync(int maNguoiDung, string matKhauCu, string matKhauMoi)
    {
        var nd = await _db.NguoiDungs.FindAsync(maNguoiDung);
        if (nd == null) return KetQua.Loi("Không tìm thấy tài khoản.");
        if (!BCrypt.Net.BCrypt.Verify(matKhauCu, nd.MatKhauHash))
            return KetQua.Loi("Mật khẩu hiện tại không đúng.");
        nd.MatKhauHash = BamMatKhau(matKhauMoi);
        await _db.SaveChangesAsync();
        return KetQua.Ok("Đổi mật khẩu thành công.");
    }

    public string BamMatKhau(string matKhau) => BCrypt.Net.BCrypt.HashPassword(matKhau);
}
