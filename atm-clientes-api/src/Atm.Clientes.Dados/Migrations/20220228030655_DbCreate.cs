using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Clientes.Dados.Migrations
{
    public partial class DbCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Placa = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    Quilometragem = table.Column<long>(type: "bigint", nullable: false),
                    Modelo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Marca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Ano = table.Column<short>(type: "smallint", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Cpf = table.Column<string>(type: "text", nullable: true),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Endereco = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarroCliente",
                columns: table => new
                {
                    CarrosId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarroCliente", x => new { x.CarrosId, x.ClientesId });
                    table.ForeignKey(
                        name: "FK_CarroCliente_Carro_CarrosId",
                        column: x => x.CarrosId,
                        principalTable: "Carro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarroCliente_Cliente_ClientesId",
                        column: x => x.ClientesId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carro_Id",
                table: "Carro",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CarroCliente_ClientesId",
                table: "CarroCliente",
                column: "ClientesId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Id",
                table: "Cliente",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarroCliente");

            migrationBuilder.DropTable(
                name: "Carro");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
