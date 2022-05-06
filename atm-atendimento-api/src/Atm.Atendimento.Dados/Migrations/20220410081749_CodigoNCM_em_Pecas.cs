using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Atendimento.Dados.Migrations
{
    public partial class CodigoNCM_em_Pecas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoNCM",
                table: "Peca",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoNCM",
                table: "Peca");
        }
    }
}
