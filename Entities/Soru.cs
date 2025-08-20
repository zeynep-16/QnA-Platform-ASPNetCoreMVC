using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Soru
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Icerik { get; set; } = string.Empty;

        public string? Cevap { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;

        // Foreign Key
        [Required]
        public int KullaniciId { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual Kullanici? Kullanici { get; set; }
    }
}
