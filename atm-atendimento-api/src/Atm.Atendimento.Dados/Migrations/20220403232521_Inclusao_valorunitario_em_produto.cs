using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Atendimento.Dados.Migrations
{
    public partial class Inclusao_valorunitario_em_produto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorUnitario",
                table: "ProdutoOrcamento",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorUnitario",
                table: "ProdutoOrcamento");
        }
    }
}
