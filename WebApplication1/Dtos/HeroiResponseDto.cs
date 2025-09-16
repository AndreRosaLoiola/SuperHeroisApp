namespace WebApplication1.Dtos;

public class HeroiResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string NomeHeroi { get; set; }
    public DateTime DataNascimento { get; set; }
    public string DataNascimentoFormatada => DataNascimento.ToString("dd/MM/yyyy");
    public double Altura { get; set; }
    public double Peso { get; set; }
    public List<SuperpoderResponseDto> Superpoderes { get; set; }
}
