namespace DTO //Data Transfer Object → Veri Transfer Nesnesi
{             //sadece ihtiyaç olan veriyi taşımak için kullanılır.
    public class SoruDTO
    {
        public string Icerik { get; set; } = string.Empty;
        public string? Cevap { get; set; } = string.Empty;
    }
}
