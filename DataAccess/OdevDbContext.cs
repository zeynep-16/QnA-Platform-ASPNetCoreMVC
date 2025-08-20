using Microsoft.EntityFrameworkCore;
using Entities;

namespace DataAccess
{
    public class OdevDbContext : DbContext
    {
        public OdevDbContext(DbContextOptions<OdevDbContext> options)
            : base(options)  //veritabanı ile bağlantı
        {
        }

        //Kullanicilar ve Sorular adlı tabloları oluşturuyoruz

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Soru> Sorular { get; set; }
    }
}
