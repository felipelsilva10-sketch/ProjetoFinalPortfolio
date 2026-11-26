namespace ProjetoFinalPortfolio.models.DTOs
{
    public class CriarAlunoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TurmaId { get; set; }
    }

    public class AlunoInfoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TurmaId { get; set; }
        public string? TurmaNome { get; set; }
    }
}