-- =====================================================================
--  HỆ THỐNG QUẢN LÝ SÁCH THƯ VIỆN HL2T
--  Script tạo cơ sở dữ liệu ThuVienHL2T_DB (MySQL 8.0+)
-- =====================================================================
DROP DATABASE IF EXISTS ThuVienHL2T_DB;
CREATE DATABASE ThuVienHL2T_DB
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;
USE ThuVienHL2T_DB;

-- ---------------------------------------------------------------------
-- 1. VaiTro
-- ---------------------------------------------------------------------
CREATE TABLE VaiTro (
    MaVaiTro   INT          NOT NULL AUTO_INCREMENT,
    TenVaiTro  VARCHAR(50)  NOT NULL,
    MoTa       VARCHAR(255) NULL,
    CONSTRAINT PK_VaiTro PRIMARY KEY (MaVaiTro),
    CONSTRAINT UQ_VaiTro_Ten UNIQUE (TenVaiTro)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- 2. NguoiDung
-- ---------------------------------------------------------------------
CREATE TABLE NguoiDung (
    MaNguoiDung  INT          NOT NULL AUTO_INCREMENT,
    TenDangNhap  VARCHAR(50)  NOT NULL,
    MatKhauHash  VARCHAR(255) NOT NULL,
    HoTen        VARCHAR(100) NOT NULL,
    Email        VARCHAR(100) NOT NULL,
    SoDienThoai  VARCHAR(15)  NULL,
    DiaChi       VARCHAR(255) NULL,
    NgayTao      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    TrangThai    TINYINT(1)   NOT NULL DEFAULT 1,
    MaVaiTro     INT          NOT NULL,
    CONSTRAINT PK_NguoiDung PRIMARY KEY (MaNguoiDung),
    CONSTRAINT UQ_NguoiDung_TenDangNhap UNIQUE (TenDangNhap),
    CONSTRAINT UQ_NguoiDung_Email UNIQUE (Email),
    CONSTRAINT FK_NguoiDung_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES VaiTro (MaVaiTro)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- 3. DanhMucSach
-- ---------------------------------------------------------------------
CREATE TABLE DanhMucSach (
    MaDanhMuc   INT          NOT NULL AUTO_INCREMENT,
    TenDanhMuc  VARCHAR(100) NOT NULL,
    MoTa        VARCHAR(255) NULL,
    CONSTRAINT PK_DanhMucSach PRIMARY KEY (MaDanhMuc),
    CONSTRAINT UQ_DanhMucSach_Ten UNIQUE (TenDanhMuc)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- 4. Sach
-- ---------------------------------------------------------------------
CREATE TABLE Sach (
    MaSach          INT           NOT NULL AUTO_INCREMENT,
    ISBN            VARCHAR(20)   NULL,
    TenSach         VARCHAR(255)  NOT NULL,
    TacGia          VARCHAR(150)  NOT NULL,
    NhaXuatBan      VARCHAR(150)  NULL,
    NamXuatBan      INT           NULL,
    GiaBia          DECIMAL(12,0) NOT NULL DEFAULT 0,
    AnhBia          VARCHAR(255)  NULL,
    MoTa            TEXT          NULL,
    TongSoLuong     INT           NOT NULL DEFAULT 0,
    SoLuongKhaDung  INT           NOT NULL DEFAULT 0,
    MaDanhMuc       INT           NOT NULL,
    CONSTRAINT PK_Sach PRIMARY KEY (MaSach),
    CONSTRAINT UQ_Sach_ISBN UNIQUE (ISBN),
    CONSTRAINT FK_Sach_DanhMucSach FOREIGN KEY (MaDanhMuc) REFERENCES DanhMucSach (MaDanhMuc),
    CONSTRAINT CK_Sach_TongSoLuong CHECK (TongSoLuong >= 0),
    CONSTRAINT CK_Sach_SoLuongKhaDung CHECK (SoLuongKhaDung >= 0 AND SoLuongKhaDung <= TongSoLuong),
    INDEX IX_Sach_TenSach (TenSach),
    INDEX IX_Sach_TacGia (TacGia)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- 5. PhieuMuon
-- ---------------------------------------------------------------------
CREATE TABLE PhieuMuon (
    MaPhieuMuon  INT          NOT NULL AUTO_INCREMENT,
    MaDocGia     INT          NOT NULL,
    MaThuThu     INT          NULL,
    NgayLap      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    NgayMuon     DATE         NULL,
    NgayHenTra   DATE         NULL,
    DaGiaHan     TINYINT(1)   NOT NULL DEFAULT 0,
    TrangThai    VARCHAR(20)  NOT NULL DEFAULT 'ChoDuyet',
    GhiChu       VARCHAR(255) NULL,
    CONSTRAINT PK_PhieuMuon PRIMARY KEY (MaPhieuMuon),
    CONSTRAINT FK_PhieuMuon_DocGia  FOREIGN KEY (MaDocGia) REFERENCES NguoiDung (MaNguoiDung),
    CONSTRAINT FK_PhieuMuon_ThuThu  FOREIGN KEY (MaThuThu) REFERENCES NguoiDung (MaNguoiDung),
    CONSTRAINT CK_PhieuMuon_TrangThai CHECK (TrangThai IN ('ChoDuyet','DangMuon','QuaHan','DaTra','DaHuy')),
    INDEX IX_PhieuMuon_TrangThai (TrangThai)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- 6. ChiTietPhieuMuon
-- ---------------------------------------------------------------------
CREATE TABLE ChiTietPhieuMuon (
    MaChiTiet        INT         NOT NULL AUTO_INCREMENT,
    MaPhieuMuon      INT         NOT NULL,
    MaSach           INT         NOT NULL,
    SoLuong          INT         NOT NULL DEFAULT 1,
    NgayTraThucTe    DATE        NULL,
    TinhTrangKhiTra  VARCHAR(50) NULL,
    CONSTRAINT PK_ChiTietPhieuMuon PRIMARY KEY (MaChiTiet),
    CONSTRAINT FK_CTPM_PhieuMuon FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon (MaPhieuMuon) ON DELETE CASCADE,
    CONSTRAINT FK_CTPM_Sach      FOREIGN KEY (MaSach) REFERENCES Sach (MaSach),
    CONSTRAINT CK_CTPM_SoLuong CHECK (SoLuong > 0)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- 7. BienLaiPhat
-- ---------------------------------------------------------------------
CREATE TABLE BienLaiPhat (
    MaBienLai    INT           NOT NULL AUTO_INCREMENT,
    MaPhieuMuon  INT           NOT NULL,
    MaDocGia     INT           NOT NULL,
    LyDo         VARCHAR(255)  NOT NULL,
    SoTien       DECIMAL(12,0) NOT NULL,
    NgayLap      DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DaThanhToan  TINYINT(1)    NOT NULL DEFAULT 0,
    CONSTRAINT PK_BienLaiPhat PRIMARY KEY (MaBienLai),
    CONSTRAINT FK_BLP_PhieuMuon FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon (MaPhieuMuon),
    CONSTRAINT FK_BLP_DocGia    FOREIGN KEY (MaDocGia) REFERENCES NguoiDung (MaNguoiDung),
    CONSTRAINT CK_BLP_SoTien CHECK (SoTien >= 0)
) ENGINE = InnoDB;

-- =====================================================================
-- DỮ LIỆU MẪU
-- Mật khẩu ghi dạng 'SEED:<mật khẩu>'; khi ứng dụng khởi động lần đầu,
-- DbSeeder sẽ tự băm lại bằng BCrypt.
--   admin / Admin@123   thuthu / ThuThu@123   docgia / DocGia@123
-- =====================================================================
INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES
 ('Admin',  'Quản trị viên hệ thống'),
 ('ThuThu', 'Thủ thư – quản lý kho sách và mượn trả'),
 ('DocGia', 'Độc giả – tra cứu và mượn sách');

INSERT INTO NguoiDung (TenDangNhap, MatKhauHash, HoTen, Email, SoDienThoai, DiaChi, MaVaiTro) VALUES
 ('admin',   'SEED:Admin@123',  'Nguyễn Văn Hùng',  'admin@hl2t.edu.vn',   '0901000001', 'Hà Nội', 1),
 ('thuthu',  'SEED:ThuThu@123', 'Trần Thu Hà',      'thuthu@hl2t.edu.vn',  '0901000002', 'Hà Nội', 2),
 ('docgia',  'SEED:DocGia@123', 'Nguyễn Minh Anh',  'docgia@hl2t.edu.vn',  '0901000003', 'Hà Nội', 3),
 ('docgia2', 'SEED:DocGia@123', 'Lê Quang Huy',     'docgia2@hl2t.edu.vn', '0901000004', 'Hà Nội', 3);

INSERT INTO DanhMucSach (TenDanhMuc, MoTa) VALUES
 ('Công nghệ thông tin', 'Lập trình, cơ sở dữ liệu, mạng máy tính'),
 ('Kinh tế',             'Kinh tế học, quản trị kinh doanh'),
 ('Kỹ năng sống',        'Kỹ năng mềm, phát triển bản thân'),
 ('Ngoại ngữ',           'Tiếng Anh, tiếng Nhật, tiếng Hàn'),
 ('Lịch sử',             'Lịch sử Việt Nam và thế giới'),
 ('Văn học',             'Tiểu thuyết, truyện ngắn, thơ');

INSERT INTO Sach (ISBN, TenSach, TacGia, NhaXuatBan, NamXuatBan, GiaBia, TongSoLuong, SoLuongKhaDung, MaDanhMuc, MoTa) VALUES
 ('978-604-0-00001-1', 'Lập trình C# từ cơ bản đến nâng cao', 'Phạm Công Ngô',   'NXB Thông tin và Truyền thông', 2024, 185000, 5, 5, 1, 'Ngôn ngữ C#, lập trình hướng đối tượng, LINQ, EF Core.'),
 ('978-604-0-00002-8', 'ASP.NET Core MVC thực chiến',         'Nguyễn Văn Hiếu', 'NXB Bách Khoa Hà Nội',          2023, 210000, 4, 4, 1, 'Xây dựng ứng dụng web theo mô hình MVC.'),
 ('978-604-0-00003-5', 'Cơ sở dữ liệu MySQL',                 'Trần Minh Đức',   'NXB Giáo dục',                   2022, 150000, 6, 6, 1, NULL),
 ('978-604-0-00004-2', 'Phân tích và thiết kế hệ thống',      'Lê Thị Hạnh',     'NXB Đại học Quốc gia Hà Nội',    2021, 120000, 8, 8, 1, NULL),
 ('978-604-0-00005-9', 'Cấu trúc dữ liệu và giải thuật',      'Đỗ Xuân Lôi',     'NXB Đại học Quốc gia Hà Nội',    2020, 135000, 10, 10, 1, NULL),
 ('978-604-0-00006-6', 'Kinh tế vĩ mô căn bản',               'Hoàng Văn Nam',   'NXB Kinh tế Quốc dân',           2022, 99000,  3, 3, 2, NULL),
 ('978-604-0-00007-3', 'Quản trị học đại cương',              'Phan Thanh Bình', 'NXB Kinh tế Quốc dân',           2021, 110000, 6, 6, 2, NULL),
 ('978-604-0-00008-0', 'Kỹ năng giao tiếp hiệu quả',          'Vũ Thu Trang',    'NXB Lao động',                   2023, 89000,  7, 7, 3, NULL),
 ('978-604-0-00009-7', 'Tiếng Anh chuyên ngành CNTT',         'Bùi Văn Tuấn',    'NXB Giáo dục',                   2022, 95000,  9, 9, 4, NULL),
 ('978-604-0-00010-3', 'Lịch sử Việt Nam giản yếu',           'Nguyễn Thị Lan',  'NXB Chính trị Quốc gia',         2019, 130000, 4, 4, 5, NULL);

-- =====================================================================
-- VIEW & THỦ TỤC HỖ TRỢ
-- =====================================================================
-- Danh sách phiếu đang mượn đã quá hạn
CREATE OR REPLACE VIEW vw_PhieuQuaHan AS
SELECT pm.MaPhieuMuon, nd.HoTen AS DocGia, pm.NgayMuon, pm.NgayHenTra,
       DATEDIFF(CURDATE(), pm.NgayHenTra) AS SoNgayTre
FROM PhieuMuon pm
JOIN NguoiDung nd ON nd.MaNguoiDung = pm.MaDocGia
WHERE pm.TrangThai IN ('DangMuon', 'QuaHan') AND pm.NgayHenTra < CURDATE();

-- Cập nhật trạng thái quá hạn và hủy phiếu chờ duyệt quá 3 ngày (QĐ7)
DELIMITER $$
CREATE PROCEDURE sp_CapNhatTrangThaiPhieu()
BEGIN
    UPDATE PhieuMuon SET TrangThai = 'QuaHan'
     WHERE TrangThai = 'DangMuon' AND NgayHenTra < CURDATE();
    UPDATE PhieuMuon SET TrangThai = 'DaHuy', GhiChu = 'Tự động hủy do quá 3 ngày chưa duyệt'
     WHERE TrangThai = 'ChoDuyet' AND NgayLap < (NOW() - INTERVAL 3 DAY);
END $$
DELIMITER ;
