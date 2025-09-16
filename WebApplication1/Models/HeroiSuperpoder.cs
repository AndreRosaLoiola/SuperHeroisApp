namespace ViceriSeidorHero.Models
{
    public class HeroiSuperpoder
    {
        public int HeroiId { get; set; }
        public Heroi Heroi { get; set; } = null!;

        public int SuperpoderId { get; set; }
        public Superpoderes Superpoder { get; set; } = null!;
    }
}
