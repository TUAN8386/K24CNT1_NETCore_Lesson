using DAT_LESSON_14_LAYOUT.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Entity Framework Core - SQL Server
builder.Services.AddDbContext<DatAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatConnection")));

var app = builder.Build();

// Tạo database + dữ liệu mẫu lần chạy đầu tiên
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatAppDbContext>();
    db.Database.EnsureCreated();
    DatSeedData.Seed(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Route cho Area (Bài 3 - Bước 10): vào /Admin sẽ mở DatDashboard/Index
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=DatDashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
