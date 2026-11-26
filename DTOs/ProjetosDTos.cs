namespace ProjetoFinalPortfolio.models.DTOs
{
    public class CriarProjetoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Tecnologias { get; set; } = string.Empty;
        public string? LinkAcesso { get; set; }
        public string? ImagemUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int AlunoId { get; set; }
    }

    public class ProjetoInfoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Tecnologias { get; set; } = string.Empty;
        public string? LinkAcesso { get; set; }
        public string? ImagemUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int AlunoId { get; set; }
        public string? AlunoNome { get; set; }
    }
}