# DAT_Layout_LESSON_13 – Lesson 13: Tìm hiểu về Layout

Sinh viên: Đinh Anh Tuấn – MSSV 2410900083
Nền tảng: ASP.NET Core MVC (.NET 10)

## Cách chạy
1. Mở `DAT_Layout_LESSON_13.csproj` bằng Visual Studio 2022 (hoặc `dotnet run` trong thư mục project).
2. Nhấn F5. Trang mặc định: `/DatProducts/DatIndex`.

## Nội dung theo từng mục của bài
| Mục trong slide | File thực hiện |
|---|---|
| Layout mặc định + `_ViewStart` | `Views/Shared/_DatLayout.cshtml`, `Views/_ViewStart.cshtml` |
| RenderBody / RenderSection (S1, S2) | `Views/DatHome/DatIndex.cshtml` → `/DatHome/DatIndex` |
| Custom Layout | `Views/Shared/_DatLayoutHome.cshtml` |
| ProductsController (Index, Search, Hots, About) | `Controllers/DatProductsController.cs` + `Views/DatProducts/*.cshtml` |
| Tạo và sử dụng CSS | `wwwroot/css/DatLayoutHome.css` |
| Tạo và sử dụng JS (link được click đổi màu) | `wwwroot/js/DatLayoutHome.js` |
| Tạo Area | `Areas/Admin/...` (`[Area("Admin")]`, route `areas` trong `Program.cs`) |
| Layout trang Admin | `Areas/Admin/Views/Shared/_DatLayoutAdmin.cshtml`, `wwwroot/css/DatLayoutAdmin.css`, `wwwroot/js/DatLayoutAdmin.js` |
| Layout trang Customer | `_DatLayoutHome.cshtml` (các trang DatProducts) |

## Các đường dẫn
- `/DatProducts/DatIndex` – Danh sách sản phẩm
- `/DatProducts/DatSearch` – Tìm kiếm sản phẩm
- `/DatProducts/DatHots` – Danh sách bán chạy
- `/DatProducts/DatAbout` – Giới thiệu
- `/DatHome/DatIndex` – Demo Layout mặc định + RenderSection
- `/Admin/DatDashboard/DatIndex` – Trang Admin (Area)
- `/Admin/DatProductManage/DatIndex` – Quản lý sản phẩm (Admin)

Ghi chú: jQuery được nạp từ CDN `code.jquery.com`, cần có mạng khi chạy.
