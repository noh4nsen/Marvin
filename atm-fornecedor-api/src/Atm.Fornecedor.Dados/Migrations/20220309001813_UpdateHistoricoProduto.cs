using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Fornecedor.Dados.Migrations
{
    public partial class UpdateHistoricoProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorCobrado",
                table: "Produto");

            migrationBuilder.RenameColumn(
                name: "ValorUnitario",
                table: "Produto",
                newName: "ValorAtual");

            migrationBuilder.AddColumn<Guid>(
                name: "HistoricoProdutoAtual",
                table: "Produto",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "HistoricoProduto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "numeric", nullable: false),
                    produtoId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("HistoricoProduto_PK", x => x.Id);
                    table.ForeignKey(
                        name: "Produto_Historic_produtoId_FK",
                        column: x => x.produtoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "HistoricoProduto_produtoId_IX",
                table: "HistoricoProduto",
                column: "produtoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoProduto");

            migrationBuilder.DropColumn(
                name: "HistoricoProdutoAtual",
                table: "Produto");

            migrationBuilder.RenameColumn(
                name: "ValorAtual",
                table: "Produto",
                newName: "ValorUnitario");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorCobrado",
                table: "Produto",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
