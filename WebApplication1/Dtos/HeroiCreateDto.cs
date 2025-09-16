using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public class HeroiCreateDto : IValidatableObject
{
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

    [Required]
    public List<int> SuperpoderesIds { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SuperpoderesIds == null || !SuperpoderesIds.Any())
        {
            yield return new ValidationResult("Pelo menos um superpoder deve ser informado.", new[] { nameof(SuperpoderesIds) });
        }
    }
}

public class HeroiUpdateDto : HeroiCreateDto { }
