namespace ProjetoFinalPortfolio.models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Tecnologias { get; set; } = string.Empty;
        public string? LinkAcesso { get; set; }
        public string? ImagemUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int AlunoId { get; set; }
        public Aluno? Aluno { get; set; }
    }
}
