using DataAccess;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Business
{
    public class SoruManager : ISoruService
    {
        private readonly OdevDbContext _context; // EF Core aracılığıyla veritabanına erişim sağlanır.Bu nesne üzerinden veri çekme, ekleme, silme işlemleri yapılır.

        public SoruManager(OdevDbContext context) //Constructor üzerinden OdevDbContext dependency injection ile alınır.
        //Yani uygulama çalışırken bu sınıf otomatik olarak ihtiyaç duyduğu veritabanı erişimini alır.
        {
            _context = context;
        }

        public async Task<List<Soru>> GetAllAsync() //Soru tablosundaki tüm verileri getirir.
        {
            return await _context.Sorular.Include(s => s.Kullanici).ToListAsync(); // her sorunun hangi kullanıcıya ait olduğu da getirilir.Kullanıldığı yer: SoruController > Index() 
        }
        
        //Belirli ID’ye Göre Soru Getirme. Veritabanından id’si verilen soruyu bulur.
        public async Task<Soru?> GetByIdAsync(int id)
        {
            return await _context.Sorular.FindAsync(id);
        }
        
        //Yeni Soru Ekleme
        //Kullanıcıdan gelen soruyu veritabanına ekler.
        //Kullanıldığı yer: Create(Soru soru) post metodu.
        public async Task AddAsync(Soru soru)
        {
            _context.Sorular.Add(soru);
            await _context.SaveChangesAsync(); //işlemi veritabanına fiziksel yazmayı sağlar.
        }

        //Soruyu Güncelleme (örneğin Cevap ekleme)
        public async Task UpdateAsync(Soru soru)
        {
            _context.Sorular.Update(soru);
            await _context.SaveChangesAsync(); 
        } //Kullanıldığı yer: Cevapla post aksiyonu.
    }
}
