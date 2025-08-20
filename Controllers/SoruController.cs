using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Entities;
using Business;

namespace OdevProjesi.Controllers
{
    public class SoruController : Controller
    {
        private readonly ISoruService _soruService;

        public SoruController(ISoruService soruService)
        {
            _soruService = soruService;
        }

        [AllowAnonymous]  //AllowAnonymous sayesinde oturum açmamış kişiler bile bu sayfayı görebilir.
        public async Task<IActionResult> Index()  //Index metodu, tüm soruları getirir ve View dosyasına yollar.
        {
            var sorular = await _soruService.GetAllAsync();
            return View(sorular);
        }

        [Authorize] //Bu sayfayı sadece giriş yapmış kullanıcılar görebilir
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]  //Bu metot, kullanıcı formu gönderdiğinde tetiklenir (POST isteği).
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Soru soru)
        {
            var kullaniciIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //Oturum açmış kullanıcının ID’sini alır.
            if (!string.IsNullOrEmpty(kullaniciIdStr) && int.TryParse(kullaniciIdStr, out int kullaniciId)) //Kullanıcı ID’si alınabiliyor mu kontrol eder.
            {
                soru.KullaniciId = kullaniciId;  //Soruyu veritabanına kaydederken kimin sorduğunu ve tarihini yazar.
                soru.Tarih = DateTime.Now;

                if (ModelState.IsValid)
                {
                    await _soruService.AddAsync(soru);
                    TempData["Mesaj"] = "Sorunuz başarıyla eklendi.";
                    return RedirectToAction(nameof(Index));  //Her şey doğruysa, soruyu ekler ve listeye döner.
                }
                else
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine("ModelState hatası: " + error.ErrorMessage);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("KullaniciId", "Kullanıcı bilgisi alınamadı.");
            }

            return View(soru);
        }

        [Authorize(Roles = "admin")] //Yalnızca "admin" rolündeki kullanıcılar bu metodu çağırabilir.
        public async Task<IActionResult> Cevapla(int id) 
        {  //Bu, cevapsız bir soruyu bulup "Cevapla" sayfasına getirir.
            var soru = await _soruService.GetByIdAsync(id);
            if (soru == null)
                return NotFound();

            return View(soru);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]  
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cevapla(int id, string cevap) //Admin cevap verdikten sonra POST isteğiyle bu metot çalışır.
        {   
            var soru = await _soruService.GetByIdAsync(id);
            if (soru == null)
                return NotFound();

            soru.Cevap = cevap;
            await _soruService.UpdateAsync(soru);

            TempData["Mesaj"] = "Cevap eklendi."; //Sorunun Cevap alanını günceller.
            return RedirectToAction(nameof(Index));  //Cevap başarılı bir şekilde eklendiyse ana sayfaya geri döner.
        }
    }
}
