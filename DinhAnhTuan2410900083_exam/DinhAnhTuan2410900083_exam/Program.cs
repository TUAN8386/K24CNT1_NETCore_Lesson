using DinhAnhTuan2410900083_exam.Data;
using Microsoft.EntityFrameworkCore;

namespace DinhAnhTuan2410900083_exam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Cấu hình kết nối MySQL
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // 2. Thêm dịch vụ Controllers với Views
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // 3. Cấu hình xử lý Request Pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Đảm bảo nạp các file static như CSS/JS
            app.UseRouting();

            app.UseAuthorization();

            // 4. Định tuyến Route mặc định
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}