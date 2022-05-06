using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Clientes.Dados.Migrations
{
    public partial class Atualização_cep_Cliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Cliente",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Cliente");
        }
    }
}
