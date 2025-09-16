using System.ComponentModel.DataAnnotations;

namespace ViceriSeidorHero.Models
{
    public class Heroi
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Nome { get; set; } = null!;

        [Required, MaxLength(120)]
        public string NomeHeroi { get; set; } = null!;

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public double Altura { get; set; }

        [Required]
        public double Peso { get; set; }

        public ICollection<HeroiSuperpoder> HeroisSuperpoderes { get; set; } = new List<HeroiSuperpoder>();
    }
}
