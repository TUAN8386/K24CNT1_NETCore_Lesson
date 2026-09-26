using HL2T.Library.Data;
using HL2T.Library.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------- Cơ sở dữ liệu MySQL (Pomelo) ----------
var cs = builder.Configuration.GetConnectionString("ThuVienHL2T")
         ?? throw new InvalidOperationException("Thiếu chuỗi kết nối 'ThuVienHL2T'.");
builder.Services.AddDbContext<ThuVienDbContext>(opt =>
    opt.UseMySql(cs, new MySqlServerVersion(new Version(8, 0, 36))));

// ---------- Quy định nghiệp vụ + các service ----------
builder.Services.Configure<QuyDinhThuVien>(builder.Configuration.GetSection("QuyDinhThuVien"));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISachService, SachService>();
builder.Services.AddScoped<IMuonTraService, MuonTraService>();
builder.Services.AddScoped<IThongKeService, ThongKeService>();

// ---------- Xác thực cookie & phân quyền theo vai trò ----------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Account/Login";
        o.LogoutPath = "/Account/Logout";
        o.AccessDeniedPath = "/Account/AccessDenied";
        o.ExpireTimeSpan = TimeSpan.FromMinutes(30);   // hết hạn sau 30 phút không hoạt động
        o.SlidingExpiration = true;
        o.Cookie.HttpOnly = true;
        o.Cookie.Name = "HL2T.Auth";
    });
builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews(o =>
    o.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute()));

var app = builder.Build();

// ---------- Khởi tạo CSDL & dữ liệu mẫu ----------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ThuVienDbContext>();
    db.Database.EnsureCreated();   // nếu đã chạy script SQL thì bỏ qua
    DbSeeder.Seed(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
