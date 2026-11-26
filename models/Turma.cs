namespace ProjetoFinalPortfolio.models
{
    public class Turma
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public List<Aluno>? Alunos { get; set; }
        public string? Periodo { get; set; }
        public string? Professor { get; set; }
    }
}