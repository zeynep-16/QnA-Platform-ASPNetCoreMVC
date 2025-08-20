using Entities;

namespace Business
{
    public interface ISoruService
    {
        Task<List<Soru>> GetAllAsync(); // Tüm soruları getirir (veritabanından List<Soru> döner).Kullanım yeri: Index.cshtml sayfası
        Task<Soru?> GetByIdAsync(int id); //Belirli bir id'ye sahip soruyu getirir.Kullanım yeri: Cevapla(int id) aksiyonu      
        Task AddAsync(Soru soru); //Yeni bir soru ekler.Kullanım yeri: Create aksiyonunun [HttpPost] versiyonu
        Task UpdateAsync(Soru soru); //Mevcut bir soruyu günceller (örneğin cevap eklerken).Kullanım yeri: Cevapla(HttpPost) aksiyonu            
    }
}
