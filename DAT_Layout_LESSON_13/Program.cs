// Đinh Anh Tuấn - 2410900083 - Lesson 13: Tìm hiểu về Layout
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/DatHome/DatError");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Route cho Area (phải khai báo TRƯỚC route mặc định)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=DatDashboard}/{action=DatIndex}/{id?}");

// Route mặc định -> trang khách hàng (Customer)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DatProducts}/{action=DatIndex}/{id?}");

app.Run();
