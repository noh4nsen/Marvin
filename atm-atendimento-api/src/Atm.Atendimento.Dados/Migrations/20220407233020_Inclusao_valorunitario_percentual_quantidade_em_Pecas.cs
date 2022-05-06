using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Atendimento.Dados.Migrations
{
    public partial class Inclusao_valorunitario_percentual_quantidade_em_Pecas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValorUnitario",
                table: "Peca",
                newName: "ValorUnitarioVenda");

            migrationBuilder.AddColumn<decimal>(
                name: "Percentual",
                table: "Peca",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "Peca",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorUnitarioCompra",
                table: "Peca",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentual",
                table: "Peca");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "Peca");

            migrationBuilder.DropColumn(
                name: "ValorUnitarioCompra",
                table: "Peca");

            migrationBuilder.RenameColumn(
                name: "ValorUnitarioVenda",
                table: "Peca",
                newName: "ValorUnitario");
        }
    }
}
