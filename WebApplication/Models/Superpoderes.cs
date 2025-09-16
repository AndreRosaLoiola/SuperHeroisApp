using System.ComponentModel.DataAnnotations;

namespace ViceriSeidorHero.Models;

public class Superpoderes
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Superpoder { get; set; } = null!;

    [Required, MaxLength(250)]
    public string Descricao { get; set; } = null!;

    public ICollection<HeroiSuperpoder> HeroisSuperpoderes { get; set; } = new List<HeroiSuperpoder>();
}
