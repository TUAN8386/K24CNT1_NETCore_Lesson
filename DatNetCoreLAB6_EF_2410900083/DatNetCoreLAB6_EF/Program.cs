using DatNetCoreLAB6_EF.Data;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

// Dùng định dạng số en-US (dấu chấm thập phân) để nhập giá/điểm không bị lỗi trên Windows tiếng Việt
var datCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = datCulture;
CultureInfo.DefaultThreadCurrentUICulture = datCulture;

var datBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.
datBuilder.Services.AddControllersWithViews();

// Cấu hình kết nối CSDL DatNetCoreCRUD (Category, Product, Banner)
var datAppConnection = datBuilder.Configuration.GetConnectionString("DatAppConnection")
    ?? throw new InvalidOperationException("Không tìm thấy chuỗi kết nối 'DatAppConnection'.");
datBuilder.Services.AddDbContext<DatAppDbContext>(datOptions =>
    datOptions.UseSqlServer(datAppConnection));

// Cấu hình kết nối CSDL DatStudentManager (StdClass, Student, Subjects, Marks)
var datStudentConnection = datBuilder.Configuration.GetConnectionString("DatStudentConnection")
    ?? throw new InvalidOperationException("Không tìm thấy chuỗi kết nối 'DatStudentConnection'.");
datBuilder.Services.AddDbContext<DatStudentDbContext>(datOptions =>
    datOptions.UseSqlServer(datStudentConnection));

var datApp = datBuilder.Build();

// Configure the HTTP request pipeline.
if (!datApp.Environment.IsDevelopment())
{
    datApp.UseExceptionHandler("/DatHome/DatError");
    datApp.UseHsts();
}

datApp.UseHttpsRedirection();
datApp.UseStaticFiles();

datApp.UseRouting();

datApp.UseAuthorization();

datApp.MapControllerRoute(
    name: "default",
    pattern: "{controller=DatHome}/{action=DatIndex}/{id?}");

datApp.Run();
