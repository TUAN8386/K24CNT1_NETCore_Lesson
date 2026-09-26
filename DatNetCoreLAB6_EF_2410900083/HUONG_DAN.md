# LabGuide06 – Thao tác dữ liệu với Entity Framework Core

Sinh viên: **Đinh Anh Tuấn** – MSSV **2410900083** – Tiền tố đặt tên: **Dat**
Nền tảng: **.NET 10**, EF Core 10, SQL Server `DOMINIC\MSSQLSERVER01` (Windows Authentication)

## 1. Nội dung đã làm

| Bài | Nội dung | Vị trí |
|---|---|---|
| Bài 1 | Cài EF Core (EntityFrameworkCore, SqlServer, Tools, Design) | `DatNetCoreLAB6_EF.csproj` |
| Bài 2 | Code-First: model `DatCategory`, `DatProduct`, `DatAppDbContext`, chuỗi kết nối | `Models/`, `Data/`, `appsettings.json`, `Program.cs` |
| Bài 3 | CRUD danh mục (bỏ CreatedDate khỏi form, tự gán `DateTime.Now`) | `DatCategoriesController`, `Views/DatCategories` |
| Tự làm 1 | CRUD sản phẩm có upload ảnh (`wwwroot/images/products`) | `DatProductsController`, `Views/DatProducts` |
| Tự làm 2 | Action `DatProduct` trong HomeController hiển thị sản phẩm dạng cột | `DatHomeController.DatProduct` |
| Tự làm 3 | CRUD Banner (Id, Name, Image, Description, CreatedDate, Status) | `DatBannersController`, `Views/DatBanners` |
| Tự làm 4 | Slider banner trên trang chủ | `Views/DatHome/DatIndex.cshtml` |
| Tự làm 5 | CSDL `DatStudentManager`: StdClass, Student, Subjects, Marks (khóa chính 2 cột) | `DatStudentDbContext` |
| Tự làm 6 | CRUD 4 bảng trên + validate đầy đủ (không rỗng, không trùng email/SĐT/tên môn, điểm 0–10, ngày sinh < hôm nay) | `DatStdClasses`, `DatStudents`, `DatSubjects`, `DatMarks` |

Quy ước đặt tên: class/controller/model/DbContext có tiền tố `Dat` (`DatCategory`, `DatAppDbContext`…), action `DatIndex/DatCreate/DatEdit/DatDetails/DatDelete`, biến `datProduct`, field `_datContext`, hằng `DAT_PRODUCT_FOLDER`, bảng `DatCategory`, `DatProduct`, `DatBanner`, `DatStdClass`, `DatStudent`, `DatSubjects`, `DatMarks`.

## 2. Cách chạy

1. Giải nén, mở **DatNetCoreLAB6_EF.slnx** bằng Visual Studio 2022 (17.14+) hoặc Visual Studio 2026.
2. Build để restore NuGet. Nếu máy chưa có bản 10.0.0, vào **Manage NuGet Packages** → Updates → cập nhật 4 gói EF Core lên cùng một bản 10.0.x.
3. Kiểm tra chuỗi kết nối trong `appsettings.json` (đang trỏ tới `DOMINIC\MSSQLSERVER01`, Windows Authentication, `TrustServerCertificate=True`).
4. Mở **Tools → NuGet Package Manager → Package Manager Console**, gõ lần lượt (có 2 DbContext nên phải ghi `-Context`):

```
Add-Migration DatV1 -Context DatAppDbContext -OutputDir Migrations/DatApp
Update-Database -Context DatAppDbContext

Add-Migration DatV1 -Context DatStudentDbContext -OutputDir Migrations/DatStudent
Update-Database -Context DatStudentDbContext
```

   EF sẽ tự tạo 2 CSDL **DatNetCoreCRUD** và **DatStudentManager** (không cần CREATE DATABASE trước).
5. Nhấn F5. Trang chủ là `/DatHome/DatIndex`.

Thứ tự nhập dữ liệu thử: Danh mục → Sản phẩm → Banner (bật Hiển thị để lên slider); Lớp → Môn học → Sinh viên → Điểm.

## 3. Lưu ý

- Bootstrap, jQuery và jQuery Validation được nạp từ CDN (cdnjs), nên máy cần có Internet khi chạy.
- Ảnh upload được đổi tên ngẫu nhiên (GUID) để không bị trùng, chỉ nhận .jpg/.jpeg/.png/.gif/.webp, tối đa 5MB. Sửa mà không chọn ảnh mới thì giữ ảnh cũ; xóa bản ghi thì xóa luôn file ảnh.
- Không cho xóa danh mục đang có sản phẩm, lớp đang có sinh viên, môn học đã có điểm. Xóa sinh viên sẽ xóa luôn điểm của sinh viên đó.
