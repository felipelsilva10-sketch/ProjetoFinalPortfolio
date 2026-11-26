namespace ProjetoFinalPortfolio.models.DTOs
{
    public class CriarTurmaDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }

    public class TurmaInfoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }
}