using Microsoft.EntityFrameworkCore;
using DATUAN_LESSON_10.Models;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký BookStoreContext sử dụng chuỗi kết nối trong appsettings.json[cite: 2]
var connectionString = builder.Configuration.GetConnectionString("BookStoreConnection");
builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");
app.Run();