using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Fornecedor.Dados.Migrations
{
    public partial class CodigoNCM_em_Produtos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoNCM",
                table: "Produto",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoNCM",
                table: "Produto");
        }
    }
}
