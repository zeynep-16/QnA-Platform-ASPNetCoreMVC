using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Entities;

namespace OdevProjesi.Controllers
{
    [Authorize]     //Bu controller'daki tüm action'lar varsayılan olarak giriş yapılmadan çalışmaz.
    public class KullaniciController : Controller
    {
        private readonly OdevDbContext _context;

        public KullaniciController(OdevDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login() => View(); //Kullanıcı giriş sayfası (Login.cshtml) görüntülenir.

        [HttpPost]
        [AllowAnonymous]   //Formdan gelen kullanıcı adı ve şifreyle giriş yapılmaya çalışılır.
        public async Task<IActionResult> Login(string kullaniciAdi, string sifre)
        { 
            var user = await _context.Kullanicilar //DB'de böyle bir kullanıcı var mı kontrol edilir.
                .FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciAdi && u.Sifre == sifre);

            if (user == null) //Kullanıcı bulunamazsa hata gösterilir.
            {
                ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }

            var claims = new List<Claim> //Kullanıcının adı, rolü ve ID'si claims'e eklenir.
            {
                new Claim(ClaimTypes.Name, user.KullaniciAdi),
                new Claim(ClaimTypes.Role, user.Rol),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) 
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            return RedirectToAction("Index", "Home"); //Giriş başarılıysa anasayfaya yönlendirilir.
        }

        [Authorize]
        public async Task<IActionResult> Logout()   //Giriş yapılmış kullanıcı çıkış yapar.
        {
            await HttpContext.SignOutAsync("MyCookieAuth"); //Cookie silinir, oturum kapanır.
            return RedirectToAction("Login", "Kullanici");
        }

        [AllowAnonymous]
        public IActionResult Register() => View(); //Yeni kullanıcı kayıt sayfası (Register.cshtml) görüntülenir.

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string kullaniciAdi, string sifre) //Formdan gelen kullanıcı adı kontrol edilir:
        {
            if (_context.Kullanicilar.Any(k => k.KullaniciAdi == kullaniciAdi))
            {
                ViewBag.Hata = "Bu kullanıcı adı zaten mevcut.";
                return View();
            }

            //Yeni kullanıcı oluşturulur, varsayılan rolü "User" olarak atanır.
            var yeniKullanici = new Kullanici
            {
                KullaniciAdi = kullaniciAdi,
                Sifre = sifre,
                Rol = "User"
            };

            _context.Kullanicilar.Add(yeniKullanici);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
    }
}

