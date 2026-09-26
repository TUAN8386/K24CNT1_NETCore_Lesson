using HL2T.Library.Data;
using HL2T.Library.Models;
using HL2T.Library.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HL2T.Library.Services;

public interface IMuonTraService
{
    Task<KetQua> KiemTraDieuKienMuonAsync(int maDocGia, int soSachMoi, int? boQuaPhieu = null);
    Task<KetQua> DangKyMuonAsync(int maDocGia, IList<int> danhSachMaSach);
    Task<KetQua> DuyetPhieuAsync(int maPhieu, int maThuThu);
    Task<KetQua> TuChoiPhieuAsync(int maPhieu, int maThuThu, string? lyDo);
    Task<KetQua> LapPhieuTaiQuayAsync(int maDocGia, IList<int> danhSachMaSach, int maThuThu);
    Task<KetQua> GiaHanAsync(int maPhieu, int? maDocGia = null);
    TienPhatDuKien TinhTienPhat(PhieuMuon phieu, IEnumerable<TraSachDongVM> dong, DateTime ngayTra);
    Task<KetQua> TraSachAsync(int maPhieu, IList<TraSachDongVM> dong, int maThuThu);
    Task<PhieuMuon?> LayPhieuAsync(int maPhieu);
    Task CapNhatTrangThaiAsync();
}

public class MuonTraService : IMuonTraService
{
    private static readonly string[] ConHieuLuc = { TrangThaiPhieu.ChoDuyet, TrangThaiPhieu.DangMuon, TrangThaiPhieu.QuaHan };

    private readonly ThuVienDbContext _db;
    private readonly QuyDinhThuVien _qd;

    public MuonTraService(ThuVienDbContext db, IOptions<QuyDinhThuVien> qd)
    {
        _db = db;
        _qd = qd.Value;
    }

    public Task<PhieuMuon?> LayPhieuAsync(int maPhieu) =>
        _db.PhieuMuons
           .Include(p => p.DocGia)
           .Include(p => p.ThuThu)
           .Include(p => p.ChiTietPhieuMuons).ThenInclude(c => c.Sach)
           .Include(p => p.BienLaiPhats)
           .FirstOrDefaultAsync(p => p.MaPhieuMuon == maPhieu);

    /// <summary>QĐ1, QĐ2: kiểm tra độc giả có đủ điều kiện mượn thêm <paramref name="soSachMoi"/> cuốn.</summary>
    public async Task<KetQua> KiemTraDieuKienMuonAsync(int maDocGia, int soSachMoi, int? boQuaPhieu = null)
    {
        var docGia = await _db.NguoiDungs.Include(n => n.VaiTro).FirstOrDefaultAsync(n => n.MaNguoiDung == maDocGia);
        if (docGia == null || docGia.VaiTro?.TenVaiTro != VaiTroNames.DocGia)
            return KetQua.Loi("Không tìm thấy độc giả.");
        if (!docGia.TrangThai)
            return KetQua.Loi("Tài khoản độc giả đang bị khóa.");

        var homNay = DateTime.Today;
        bool coQuaHan = await _db.PhieuMuons.AnyAsync(p => p.MaDocGia == maDocGia
            && (p.TrangThai == TrangThaiPhieu.QuaHan
                || (p.TrangThai == TrangThaiPhieu.DangMuon && p.NgayHenTra < homNay)));
        if (coQuaHan)
            return KetQua.Loi("Độc giả đang có phiếu mượn quá hạn chưa trả.");

        bool noPhat = await _db.BienLaiPhats.AnyAsync(b => b.MaDocGia == maDocGia && !b.DaThanhToan);
        if (noPhat)
            return KetQua.Loi("Độc giả còn biên lai phạt chưa thanh toán.");

        int dangMuon = await _db.ChiTietPhieuMuons
            .Where(c => c.PhieuMuon!.MaDocGia == maDocGia
                        && ConHieuLuc.Contains(c.PhieuMuon.TrangThai)
                        && c.NgayTraThucTe == null
                        && (boQuaPhieu == null || c.MaPhieuMuon != boQuaPhieu))
            .SumAsync(c => (int?)c.SoLuong) ?? 0;

        if (dangMuon + soSachMoi > _qd.SoSachMuonToiDa)
            return KetQua.Loi($"Vượt quá số sách được mượn. Độc giả đang mượn/chờ duyệt {dangMuon} cuốn, " +
                              $"chỉ được mượn thêm tối đa {Math.Max(0, _qd.SoSachMuonToiDa - dangMuon)} cuốn.");

        return KetQua.Ok($"Độc giả đủ điều kiện mượn (tổng {dangMuon + soSachMoi}/{_qd.SoSachMuonToiDa} cuốn).");
    }

    public async Task<KetQua> DangKyMuonAsync(int maDocGia, IList<int> danhSachMaSach)
    {
        var ds = danhSachMaSach.Distinct().ToList();
        if (ds.Count == 0) return KetQua.Loi("Chưa chọn sách cần mượn.");

        var dk = await KiemTraDieuKienMuonAsync(maDocGia, ds.Count);
        if (!dk.ThanhCong) return dk;

        var sachs = await _db.Sachs.Where(s => ds.Contains(s.MaSach)).ToListAsync();
        if (sachs.Count != ds.Count) return KetQua.Loi("Có sách không tồn tại.");
        var het = sachs.FirstOrDefault(s => !s.KiemTraKhaDung());
        if (het != null) return KetQua.Loi($"Sách \"{het.TenSach}\" hiện đã hết, vui lòng chọn sách khác.");

        var phieu = new PhieuMuon
        {
            MaDocGia = maDocGia,
            NgayLap = DateTime.Now,
            TrangThai = TrangThaiPhieu.ChoDuyet,
            ChiTietPhieuMuons = ds.Select(m => new ChiTietPhieuMuon { MaSach = m, SoLuong = 1 }).ToList()
        };
        _db.PhieuMuons.Add(phieu);
        await _db.SaveChangesAsync();
        return KetQua.Ok($"Đăng ký mượn thành công. Mã phiếu {phieu.MaHienThi} đang chờ thủ thư duyệt.", phieu.MaPhieuMuon);
    }

    public async Task<KetQua> DuyetPhieuAsync(int maPhieu, int maThuThu)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        var phieu = await _db.PhieuMuons.Include(p => p.ChiTietPhieuMuons).ThenInclude(c => c.Sach)
                                        .FirstOrDefaultAsync(p => p.MaPhieuMuon == maPhieu);
        if (phieu == null) return KetQua.Loi("Không tìm thấy phiếu mượn.");
        if (phieu.TrangThai != TrangThaiPhieu.ChoDuyet) return KetQua.Loi("Phiếu không ở trạng thái chờ duyệt.");

        var dk = await KiemTraDieuKienMuonAsync(phieu.MaDocGia, phieu.ChiTietPhieuMuons.Sum(c => c.SoLuong), boQuaPhieu: maPhieu);
        if (!dk.ThanhCong) return dk;

        try
        {
            foreach (var ct in phieu.ChiTietPhieuMuons)
                ct.Sach!.GiamSoLuong(ct.SoLuong);
        }
        catch (InvalidOperationException ex)
        {
            return KetQua.Loi(ex.Message);
        }

        phieu.TrangThai = TrangThaiPhieu.DangMuon;
        phieu.MaThuThu = maThuThu;
        phieu.NgayMuon = DateTime.Today;
        phieu.NgayHenTra = DateTime.Today.AddDays(_qd.SoNgayMuon);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return KetQua.Ok($"Đã duyệt phiếu {phieu.MaHienThi}. Hạn trả: {phieu.NgayHenTra:dd/MM/yyyy}.", phieu.MaPhieuMuon);
    }

    public async Task<KetQua> TuChoiPhieuAsync(int maPhieu, int maThuThu, string? lyDo)
    {
        var phieu = await _db.PhieuMuons.FindAsync(maPhieu);
        if (phieu == null) return KetQua.Loi("Không tìm thấy phiếu mượn.");
        if (phieu.TrangThai != TrangThaiPhieu.ChoDuyet) return KetQua.Loi("Chỉ từ chối được phiếu đang chờ duyệt.");
        phieu.TrangThai = TrangThaiPhieu.DaHuy;
        phieu.MaThuThu = maThuThu;
        phieu.GhiChu = string.IsNullOrWhiteSpace(lyDo) ? "Thủ thư từ chối" : lyDo.Trim();
        await _db.SaveChangesAsync();
        return KetQua.Ok($"Đã từ chối phiếu {phieu.MaHienThi}.");
    }

    public async Task<KetQua> LapPhieuTaiQuayAsync(int maDocGia, IList<int> danhSachMaSach, int maThuThu)
    {
        var kq = await DangKyMuonAsync(maDocGia, danhSachMaSach);
        if (!kq.ThanhCong || kq.Ma == null) return kq;
        var duyet = await DuyetPhieuAsync(kq.Ma.Value, maThuThu);
        if (!duyet.ThanhCong)
        {
            // hoàn tác phiếu vừa tạo nếu không duyệt được
            var p = await _db.PhieuMuons.FindAsync(kq.Ma.Value);
            if (p != null) { _db.PhieuMuons.Remove(p); await _db.SaveChangesAsync(); }
        }
        return duyet;
    }

    /// <summary>QĐ3: gia hạn 1 lần, thêm 7 ngày, khi phiếu chưa quá hạn.</summary>
    public async Task<KetQua> GiaHanAsync(int maPhieu, int? maDocGia = null)
    {
        var phieu = await _db.PhieuMuons.FindAsync(maPhieu);
        if (phieu == null || (maDocGia.HasValue && phieu.MaDocGia != maDocGia))
            return KetQua.Loi("Không tìm thấy phiếu mượn.");
        if (phieu.TrangThai != TrangThaiPhieu.DangMuon) return KetQua.Loi("Chỉ gia hạn được phiếu đang mượn.");
        if (phieu.KiemTraQuaHan()) return KetQua.Loi("Phiếu đã quá hạn, không thể gia hạn.");
        if (phieu.DaGiaHan) return KetQua.Loi("Phiếu đã được gia hạn một lần.");

        phieu.NgayHenTra = phieu.NgayHenTra!.Value.AddDays(_qd.SoNgayGiaHan);
        phieu.DaGiaHan = true;
        await _db.SaveChangesAsync();
        return KetQua.Ok($"Gia hạn thành công. Hạn trả mới: {phieu.NgayHenTra:dd/MM/yyyy}.");
    }

    /// <summary>QĐ4, QĐ5: phạt trễ hạn theo ngày và bồi thường theo tình trạng sách.</summary>
    public TienPhatDuKien TinhTienPhat(PhieuMuon phieu, IEnumerable<TraSachDongVM> dong, DateTime ngayTra)
    {
        var kq = new TienPhatDuKien { SoNgayTre = phieu.SoNgayTre(ngayTra) };
        foreach (var d in dong.Where(x => x.ChonTra))
        {
            if (d.TinhTrang != TinhTrangSach.Mat)
                kq.PhatTreHan += kq.SoNgayTre * _qd.TienPhatMoiNgay * d.SoLuong;
            kq.BoiThuong += Math.Round(d.GiaBia * TinhTrangSach.TyLeBoiThuong(d.TinhTrang)) * d.SoLuong;
        }
        return kq;
    }

    public async Task<KetQua> TraSachAsync(int maPhieu, IList<TraSachDongVM> dong, int maThuThu)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        var phieu = await _db.PhieuMuons.Include(p => p.ChiTietPhieuMuons).ThenInclude(c => c.Sach)
                                        .FirstOrDefaultAsync(p => p.MaPhieuMuon == maPhieu);
        if (phieu == null) return KetQua.Loi("Không tìm thấy phiếu mượn.");
        if (phieu.TrangThai != TrangThaiPhieu.DangMuon && phieu.TrangThai != TrangThaiPhieu.QuaHan)
            return KetQua.Loi("Phiếu không ở trạng thái đang mượn.");

        var ngayTra = DateTime.Today;
        var chon = dong.Where(d => d.ChonTra).ToList();
        if (chon.Count == 0) return KetQua.Loi("Chưa chọn sách trả.");

        var lyDo = new List<string>();
        foreach (var d in chon)
        {
            var ct = phieu.ChiTietPhieuMuons.FirstOrDefault(c => c.MaChiTiet == d.MaChiTiet && !c.DaTra);
            if (ct == null) continue;
            // lấy giá bìa, số lượng từ CSDL thay vì tin dữ liệu gửi lên
            d.GiaBia = ct.Sach!.GiaBia;
            d.SoLuong = ct.SoLuong;
            d.TenSach = ct.Sach.TenSach;

            ct.NgayTraThucTe = ngayTra;
            ct.TinhTrangKhiTra = TinhTrangSach.TatCa.Contains(d.TinhTrang) ? d.TinhTrang : TinhTrangSach.BinhThuong;

            if (ct.TinhTrangKhiTra == TinhTrangSach.Mat)
            {
                // sách mất: giảm tổng số lượng, số khả dụng giữ nguyên
                ct.Sach.TongSoLuong -= ct.SoLuong;
                lyDo.Add($"Mất sách \"{ct.Sach.TenSach}\"");
            }
            else
            {
                ct.Sach.TangSoLuong(ct.SoLuong);
                if (ct.TinhTrangKhiTra != TinhTrangSach.BinhThuong)
                    lyDo.Add($"{ct.TinhTrangKhiTra} \"{ct.Sach.TenSach}\"");
            }
        }

        var tien = TinhTienPhat(phieu, chon, ngayTra);
        if (tien.SoNgayTre > 0) lyDo.Insert(0, $"Trả trễ {tien.SoNgayTre} ngày");

        if (phieu.ChiTietPhieuMuons.All(c => c.DaTra))
            phieu.TrangThai = TrangThaiPhieu.DaTra;

        BienLaiPhat? bl = null;
        if (tien.Tong > 0)
        {
            bl = new BienLaiPhat
            {
                MaPhieuMuon = phieu.MaPhieuMuon,
                MaDocGia = phieu.MaDocGia,
                SoTien = tien.Tong,
                LyDo = string.Join("; ", lyDo),
                NgayLap = DateTime.Now
            };
            _db.BienLaiPhats.Add(bl);
        }

        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        var msg = $"Đã ghi nhận trả {chon.Count} đầu sách của phiếu {phieu.MaHienThi}.";
        if (bl != null) msg += $" Lập biên lai phạt {bl.MaHienThi}: {bl.SoTien:N0}đ.";
        return KetQua.Ok(msg, bl?.MaBienLai);
    }

    /// <summary>Chuyển phiếu quá hạn sang QuaHan; hủy phiếu chờ duyệt quá hạn xử lý (QĐ7).</summary>
    public async Task CapNhatTrangThaiAsync()
    {
        var homNay = DateTime.Today;
        var quaHan = await _db.PhieuMuons
            .Where(p => p.TrangThai == TrangThaiPhieu.DangMuon && p.NgayHenTra < homNay).ToListAsync();
        quaHan.ForEach(p => p.TrangThai = TrangThaiPhieu.QuaHan);

        var moc = DateTime.Now.AddDays(-_qd.SoNgayHuyPhieuChoDuyet);
        var huy = await _db.PhieuMuons
            .Where(p => p.TrangThai == TrangThaiPhieu.ChoDuyet && p.NgayLap < moc).ToListAsync();
        foreach (var p in huy)
        {
            p.TrangThai = TrangThaiPhieu.DaHuy;
            p.GhiChu = $"Tự động hủy do quá {_qd.SoNgayHuyPhieuChoDuyet} ngày chưa được duyệt";
        }

        if (quaHan.Count + huy.Count > 0) await _db.SaveChangesAsync();
    }
}
