using Microsoft.EntityFrameworkCore;
using DataAccess;
using Middleware; // Custom middleware
using Business;   // Business katmanı

var builder = WebApplication.CreateBuilder(args);

//  Business Katmanını ekle
builder.Services.AddScoped<ISoruService, SoruManager>();

//  Cookie tabanlı oturum yönetimi
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Kullanici/Login";
        options.AccessDeniedPath = "/Kullanici/AccessDenied";
    });
    //Kullanıcı giriş yaptıktan sonra tarayıcıya cookie yazılır.
    //Eğer kullanıcı giriş yapmadıysa → /Kullanici/Login sayfasına yönlendirilir.

//  Veritabanı bağlantısı (EF Core)
builder.Services.AddDbContext<OdevDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  MVC ve API Controller desteği
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // API için şart

var app = builder.Build();

//  Hata yönetimi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//  Custom middleware (istek loglama)
app.UseMiddleware<RequestLogMiddleware>();

app.UseRouting();

//  Kimlik doğrulama ve yetkilendirme
app.UseAuthentication();
app.UseAuthorization();

//  MVC controller varsayılan route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//  API controllerlar için route
app.MapControllers();

app.Run();
