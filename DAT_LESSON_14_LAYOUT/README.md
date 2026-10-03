# DAT_LESSON_14_LAYOUT — Tìm hiểu về Layout (ASP.NET Core MVC, .NET 10)

Sinh viên: Đinh Anh Tuấn — MSSV 2410900083

## 1. Cách chạy

1. Visual Studio 2026 → tạo project **ASP.NET Core Web App (Model-View-Controller)**, framework **.NET 10**, tên **DAT_LESSON_14_LAYOUT**.
   (Bước này để có sẵn `wwwroot/lib` gồm bootstrap, jquery, jquery-validation.)
2. Giải nén zip, mở thư mục `DAT_LESSON_14_LAYOUT/DAT_LESSON_14_LAYOUT/` trong zip (chỗ có file `.csproj`), **copy toàn bộ nội dung** vào thư mục project vừa tạo (nơi chứa `.csproj` của bạn), chọn *Replace*.
3. Tải AdminLTE 3: https://github.com/ColorlibHQ/AdminLTE/releases (bản 3.2.x) → giải nén → copy **2 thư mục `dist` và `plugins`** vào `wwwroot`.
4. Sửa chuỗi kết nối trong `appsettings.json` nếu SQL Server của bạn không phải `Server=.`
   (ví dụ `Server=.\\SQLEXPRESS;...`).
5. Chạy (F5). Lần đầu chạy app tự tạo database `DatLesson14Db` + dữ liệu mẫu.
   Không cần `Add-Migration`. File `DatLesson14.sql` chỉ để tham khảo/nộp bài.

## 2. Đường dẫn

| Trang | URL |
|---|---|
| Trang chủ (layout `_DatMain`) | `/` |
| Layout gốc `_Layout` (so sánh) | `/Home/Privacy` |
| Admin Dashboard | `/Admin` |
| CRUD | `/Admin/DatCategory`, `/Admin/DatProduct`, `/Admin/DatBanner`, `/Admin/DatBlog` |

## 3. Đối chiếu với bài lab

| Bài | Nội dung | File |
|---|---|---|
| Bài 1 | `_Layout`, `_ViewImports`, `_ViewStart`, `@RenderBody`, `@RenderSection` | `Views/Shared/_Layout.cshtml` (có chú thích 6 phần), `Views/_ViewImports.cshtml`, `Views/Home/Privacy.cshtml` |
| Bài 2 | Tạo layout mới, áp dụng cho 1 view và mặc định cho dự án | `Views/Shared/_DatMain.cshtml`, `Views/Home/Index.cshtml` (Layout = ...), `Views/_ViewStart.cshtml` (Layout = "_DatMain") |
| Bài 3 | Tạo Area Admin, layout riêng, Dashboard mặc định, Category | `Areas/Admin/...`, route `areas` trong `Program.cs` |
| Bài 3 (template) | Layout người dùng từ HTML template: header/menu/footer giữ lại, banner+main thay bằng `@RenderBody()` | `_DatMain.cshtml`, `wwwroot/css/dat-layout.css`, `Views/Home/Index.cshtml` |
| Tự làm 1 | Layout Admin từ AdminLTE 3, chia nhỏ bằng component | `Areas/Admin/Views/Shared/_DatAdmin.cshtml`, `Areas/Admin/ViewComponents/DatHeader, DatNavLeft, DatFooter` |
| Tự làm 2 | CRUD Category, Product, Banner, Blog + validate | `Areas/Admin/Controllers/*`, `Models/*`, `Data/DatAppDbContext.cs` |

**Ghi chú:**
- Các file tự tạo đều có tiền tố `Dat`. Các file bắt buộc theo quy ước của ASP.NET Core giữ nguyên tên:
  `Program.cs`, `_ViewImports.cshtml`, `_ViewStart.cshtml`, `_Layout.cshtml`, `HomeController`, `Error.cshtml`, `_ValidationScriptsPartial.cshtml`.
- Template "Bistup" trong link Google Drive của lab: mình dựng lại cùng cấu trúc (topbar, header, menu, banner, footer) bằng `dat-layout.css`.
  Nếu muốn dùng đúng template gốc: copy `css`, `images`, `js` của template vào `wwwroot` và đổi `href` trong `_DatMain.cshtml` thành `~/css/layout.css`.
- Cột `Prioty` trong đề được đặt tên chuẩn là `Priority`.

## 4. Validate đã làm

- Name: bắt buộc, ≤ 100 ký tự, **không trùng** (kiểm tra ở controller + unique index trong DB).
- Status: 0/1, mặc định 1. CreatedDate: mặc định ngày hiện tại.
- Product: Price bắt buộc, ≥ 0; SalePrice ≥ 0, mặc định 0, không lớn hơn Price; CategoryId bắt buộc và phải tồn tại.
- Image: upload .jpg/.jpeg/.png/.gif/.webp ≤ 2MB, lưu `wwwroot/uploads/...` (đường dẫn ≤ 100 ký tự).
- Description ≤ 350 ký tự.
- Không cho xóa danh mục còn sản phẩm.
- Validate cả phía client (jQuery Validation Unobtrusive) và server.
