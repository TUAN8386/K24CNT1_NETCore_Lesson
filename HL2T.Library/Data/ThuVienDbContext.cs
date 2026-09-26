using HL2T.Library.Models;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Data;

public class ThuVienDbContext : DbContext
{
    public ThuVienDbContext(DbContextOptions<ThuVienDbContext> options) : base(options) { }

    public DbSet<VaiTro> VaiTros => Set<VaiTro>();
    public DbSet<NguoiDung> NguoiDungs => Set<NguoiDung>();
    public DbSet<DanhMucSach> DanhMucSachs => Set<DanhMucSach>();
    public DbSet<Sach> Sachs => Set<Sach>();
    public DbSet<PhieuMuon> PhieuMuons => Set<PhieuMuon>();
    public DbSet<ChiTietPhieuMuon> ChiTietPhieuMuons => Set<ChiTietPhieuMuon>();
    public DbSet<BienLaiPhat> BienLaiPhats => Set<BienLaiPhat>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<VaiTro>().HasIndex(x => x.TenVaiTro).IsUnique();

        b.Entity<NguoiDung>(e =>
        {
            e.HasIndex(x => x.TenDangNhap).IsUnique();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.NgayTao).HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(x => x.TrangThai).HasDefaultValue(true);
            e.HasOne(x => x.VaiTro).WithMany(v => v.NguoiDungs)
             .HasForeignKey(x => x.MaVaiTro).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<DanhMucSach>().HasIndex(x => x.TenDanhMuc).IsUnique();

        b.Entity<Sach>(e =>
        {
            e.HasIndex(x => x.ISBN).IsUnique();
            e.HasIndex(x => x.TenSach);
            e.HasIndex(x => x.TacGia);
            e.Property(x => x.MoTa).HasColumnType("text");
            e.HasOne(x => x.DanhMucSach).WithMany(d => d.Sachs)
             .HasForeignKey(x => x.MaDanhMuc).OnDelete(DeleteBehavior.Restrict);
            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Sach_TongSoLuong", "TongSoLuong >= 0");
                t.HasCheckConstraint("CK_Sach_SoLuongKhaDung", "SoLuongKhaDung >= 0 AND SoLuongKhaDung <= TongSoLuong");
            });
        });

        b.Entity<PhieuMuon>(e =>
        {
            e.Property(x => x.NgayLap).HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(x => x.TrangThai).HasDefaultValue(TrangThaiPhieu.ChoDuyet);
            e.HasIndex(x => x.TrangThai);
            e.HasOne(x => x.DocGia).WithMany(n => n.PhieuMuons)
             .HasForeignKey(x => x.MaDocGia).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.ThuThu).WithMany()
             .HasForeignKey(x => x.MaThuThu).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ChiTietPhieuMuon>(e =>
        {
            e.HasOne(x => x.PhieuMuon).WithMany(p => p.ChiTietPhieuMuons)
             .HasForeignKey(x => x.MaPhieuMuon).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Sach).WithMany(s => s.ChiTietPhieuMuons)
             .HasForeignKey(x => x.MaSach).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<BienLaiPhat>(e =>
        {
            e.Property(x => x.NgayLap).HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.HasOne(x => x.PhieuMuon).WithMany(p => p.BienLaiPhats)
             .HasForeignKey(x => x.MaPhieuMuon).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.DocGia).WithMany(n => n.BienLaiPhats)
             .HasForeignKey(x => x.MaDocGia).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
