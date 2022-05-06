using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Fornecedor.Dados.Migrations
{
    public partial class Inclusao_de_variavel_de_Ativo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Produto_Historic_produtoId_FK",
                table: "HistoricoProduto");

            migrationBuilder.RenameColumn(
                name: "produtoId",
                table: "HistoricoProduto",
                newName: "ProdutoId");

            migrationBuilder.RenameIndex(
                name: "HistoricoProduto_produtoId_IX",
                table: "HistoricoProduto",
                newName: "HistoricoProduto_ProdutoId_IX");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Produto",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "HistoricoProduto",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Fornecedor",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "Produto_Historic_ProdutoId_FK",
                table: "HistoricoProduto",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Produto_Historic_ProdutoId_FK",
                table: "HistoricoProduto");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "HistoricoProduto");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Fornecedor");

            migrationBuilder.RenameColumn(
                name: "ProdutoId",
                table: "HistoricoProduto",
                newName: "produtoId");

            migrationBuilder.RenameIndex(
                name: "HistoricoProduto_ProdutoId_IX",
                table: "HistoricoProduto",
                newName: "HistoricoProduto_produtoId_IX");

            migrationBuilder.AddForeignKey(
                name: "Produto_Historic_produtoId_FK",
                table: "HistoricoProduto",
                column: "produtoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
