using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoFinalPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Periodo",
                table: "Turmas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Professor",
                table: "Turmas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Matricula",
                table: "Alunos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Periodo",
                table: "Turmas");

            migrationBuilder.DropColumn(
                name: "Professor",
                table: "Turmas");

            migrationBuilder.DropColumn(
                name: "Matricula",
                table: "Alunos");
        }
    }
}
