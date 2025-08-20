using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Kullanici
    {
        public int Id { get; set; }

        [Required]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required]
        public string Sifre { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty;

        // Bu kullanıcıya ait yazdığı sorular
        public virtual ICollection<Soru>? Sorular { get; set; }
    }
}
