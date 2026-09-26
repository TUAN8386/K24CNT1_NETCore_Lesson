# HL2T.Library – Hệ thống Quản lý Sách Thư viện HL2T

Mã nguồn ứng dụng web minh họa cho đồ án **Phân tích và thiết kế hệ thống Quản lý Sách Thư viện HL2T**.

- Nền tảng: **ASP.NET Core MVC (.NET 8)**, C# 12
- Truy cập dữ liệu: **Entity Framework Core 8** + **Pomelo.EntityFrameworkCore.MySql**
- Cơ sở dữ liệu: **MySQL 8** – `ThuVienHL2T_DB`
- Giao diện: Razor View + Bootstrap 5, Bootstrap Icons
- Bảo mật: Cookie Authentication, phân quyền theo vai trò, mật khẩu băm **BCrypt**, chống CSRF (AutoValidateAntiforgeryToken)
- Xuất báo cáo Excel: **ClosedXML**

## 1. Cấu trúc thư mục

```
HL2T.Library/
├── Database/ThuVienHL2T_DB.sql      # Script tạo CSDL, ràng buộc, dữ liệu mẫu, view, procedure
├── Models/                          # 7 lớp thực thể: VaiTro, NguoiDung, DanhMucSach, Sach,
│                                    #   PhieuMuon, ChiTietPhieuMuon, BienLaiPhat (+ hằng số trạng thái)
├── ViewModels/                      # Dữ liệu cho form / màn hình
├── Data/ThuVienDbContext.cs         # DbContext + cấu hình Fluent API
├── Data/DbSeeder.cs                 # Dữ liệu khởi tạo, băm mật khẩu mẫu
├── Services/                        # Tầng nghiệp vụ
│   ├── AuthService.cs               #   Đăng nhập, đổi mật khẩu
│   ├── SachService.cs               #   Tra cứu, thêm/sửa/xóa sách, tải ảnh bìa
│   ├── MuonTraService.cs            #   Đăng ký mượn, duyệt, gia hạn, trả sách, tính phạt (QĐ1–QĐ7)
│   └── ThongKeService.cs            #   Dashboard, thống kê, xuất Excel
├── Controllers/                     # Phân hệ Độc giả: Home, Account, Sach, PhieuMuon
├── Areas/Admin/Controllers/         # Phân hệ Quản trị: Dashboard, DanhMuc, Sach, PhieuMuon,
│                                    #   BienLai, NguoiDung, ThongKe
├── Views/, Areas/Admin/Views/       # Giao diện Razor
└── wwwroot/                         # CSS, ảnh bìa sách tải lên
```

## 2. Cài đặt và chạy

**Yêu cầu:** .NET SDK 8.0, MySQL Server 8.0, Visual Studio 2022 (hoặc VS Code).

1. **Tạo cơ sở dữ liệu** – mở MySQL Workbench và chạy file `Database/ThuVienHL2T_DB.sql`.
   *(Có thể bỏ qua bước này: khi chạy lần đầu, ứng dụng tự tạo CSDL bằng `EnsureCreated()` và thêm dữ liệu mẫu tối thiểu.)*
2. **Sửa chuỗi kết nối** trong `appsettings.json` cho đúng tài khoản MySQL của bạn:
   ```json
   "ThuVienHL2T": "Server=localhost;Port=3306;Database=ThuVienHL2T_DB;User=root;Password=<mật khẩu>;CharSet=utf8mb4;"
   ```
3. **Chạy ứng dụng:**
   ```bash
   dotnet restore
   dotnet run
   ```
   hoặc mở `HL2T.Library.csproj` bằng Visual Studio và nhấn F5. Truy cập `https://localhost:7125`.

## 3. Tài khoản mẫu

| Vai trò        | Tên đăng nhập | Mật khẩu    |
|----------------|---------------|-------------|
| Quản trị viên  | `admin`       | `Admin@123` |
| Thủ thư        | `thuthu`      | `ThuThu@123`|
| Độc giả        | `docgia`      | `DocGia@123`|

Trong script SQL, mật khẩu mẫu được lưu dạng `SEED:<mật khẩu>`; lần khởi động đầu tiên `DbSeeder` sẽ tự băm lại bằng BCrypt.
**Hãy đổi mật khẩu sau khi đăng nhập.**

## 4. Chức năng theo vai trò

| Chức năng (use case)            | Độc giả | Thủ thư | Quản trị viên |
|---------------------------------|:------:|:------:|:-------------:|
| Đăng nhập / đăng xuất, đổi mật khẩu, thông tin cá nhân | ✔ | ✔ | ✔ |
| Tra cứu, xem chi tiết sách      | ✔ | ✔ | ✔ |
| Đăng ký mượn, gia hạn, xem lịch sử mượn trả | ✔ |  |  |
| Quản lý danh mục, quản lý sách  |  | ✔ | ✔ |
| Duyệt / từ chối / lập phiếu mượn, gia hạn |  | ✔ | ✔ |
| Trả sách, tính phạt, lập & thu biên lai phạt |  | ✔ | ✔ |
| Thống kê, báo cáo, xuất Excel   |  | ✔ | ✔ |
| Quản lý người dùng, phân quyền, khóa tài khoản |  |  | ✔ |

## 5. Quy định nghiệp vụ (cấu hình trong `appsettings.json` → `QuyDinhThuVien`)

| Mã  | Quy định | Vị trí xử lý |
|-----|----------|--------------|
| QĐ1 | Tối đa 5 cuốn/độc giả, hạn mượn 14 ngày | `MuonTraService.KiemTraDieuKienMuonAsync`, `DuyetPhieuAsync` |
| QĐ2 | Có phiếu quá hạn / biên lai chưa nộp thì không được mượn | `KiemTraDieuKienMuonAsync` |
| QĐ3 | Gia hạn 1 lần, thêm 7 ngày, khi chưa quá hạn | `GiaHanAsync` |
| QĐ4 | Phạt trễ hạn 5.000đ/cuốn/ngày | `TinhTienPhat` |
| QĐ5 | Bồi thường: hư hỏng nhẹ 10%, nặng 50%, mất sách 100% giá bìa | `TinhTrangSach.TyLeBoiThuong` |
| QĐ6 | Không xóa sách/danh mục đang được sử dụng | `SachService.XoaAsync`, `DanhMucController.Delete` |
| QĐ7 | Phiếu chờ duyệt quá 3 ngày tự động hủy | `CapNhatTrangThaiAsync`, `sp_CapNhatTrangThaiPhieu` |

Các thao tác duyệt phiếu và trả sách được thực hiện trong **transaction** để đảm bảo `SoLuongKhaDung` luôn nhất quán
(ràng buộc CHECK `0 ≤ SoLuongKhaDung ≤ TongSoLuong`).

## 6. Ghi chú so với thiết kế trong báo cáo

- Bảng `Sach` bổ sung cột `MoTa` (giới thiệu sách) và bảng `PhieuMuon` bổ sung cột `DaGiaHan` (phục vụ QĐ3).
- `DbContext` của EF Core đóng vai trò tầng truy cập dữ liệu (Repository/Unit of Work), các Service gọi trực tiếp `ThuVienDbContext`.
