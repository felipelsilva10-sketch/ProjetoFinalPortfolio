namespace ProjetoFinalPortfolio.models
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TurmaId { get; set; }
        public Turma? Turma { get; set; }
        public List<Projeto>? Projetos { get; set; }
    }
}
