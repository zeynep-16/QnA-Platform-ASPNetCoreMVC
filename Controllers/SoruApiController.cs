using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace OdevProjesi.Controllers
{
    [Route("api/[controller]")] //HTML View döndürmez, sadece veri döner.
    [ApiController]    //Veritabanında kayıtlı soruları başka sistemlere veya kullanıcılara JSON veri olarak sunar
    public class SoruApiController : ControllerBase
    {
        private readonly OdevDbContext _context;

        public SoruApiController(OdevDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSorular() //Soru Listesini Getirir
        {
            var sorular = await _context.Sorular
                .Include(s => s.Kullanici)
                .Select(s => new
                {
                    s.Id,
                    s.Icerik,
                    s.Cevap,
                    s.Tarih,
                    KullaniciAdi = s.Kullanici != null ? s.Kullanici.KullaniciAdi : "anonim"
                })
                .ToListAsync();

            return Ok(sorular);
        }
    }
}
