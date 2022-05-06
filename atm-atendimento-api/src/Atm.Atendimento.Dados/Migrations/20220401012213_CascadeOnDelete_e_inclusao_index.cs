using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Atendimento.Dados.Migrations
{
    public partial class CascadeOnDelete_e_inclusao_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustoServico_Orcamento_OrcamentoId",
                table: "CustoServico");

            migrationBuilder.DropForeignKey(
                name: "FK_Peca_Orcamento_OrcamentoId",
                table: "Peca");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoOrcamento_Orcamento_OrcamentoId",
                table: "ProdutoOrcamento");

            migrationBuilder.CreateIndex(
                name: "IX_Servico_Id",
                table: "Servico",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Peca_Id",
                table: "Peca",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orcamento_Id",
                table: "Orcamento",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustoServico_Id",
                table: "CustoServico",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustoServico_Orcamento_OrcamentoId",
                table: "CustoServico",
                column: "OrcamentoId",
                principalTable: "Orcamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Peca_Orcamento_OrcamentoId",
                table: "Peca",
                column: "OrcamentoId",
                principalTable: "Orcamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoOrcamento_Orcamento_OrcamentoId",
                table: "ProdutoOrcamento",
                column: "OrcamentoId",
                principalTable: "Orcamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustoServico_Orcamento_OrcamentoId",
                table: "CustoServico");

            migrationBuilder.DropForeignKey(
                name: "FK_Peca_Orcamento_OrcamentoId",
                table: "Peca");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoOrcamento_Orcamento_OrcamentoId",
                table: "ProdutoOrcamento");

            migrationBuilder.DropIndex(
                name: "IX_Servico_Id",
                table: "Servico");

            migrationBuilder.DropIndex(
                name: "IX_Peca_Id",
                table: "Peca");

            migrationBuilder.DropIndex(
                name: "IX_Orcamento_Id",
                table: "Orcamento");

            migrationBuilder.DropIndex(
                name: "IX_CustoServico_Id",
                table: "CustoServico");

            migrationBuilder.AddForeignKey(
                name: "FK_CustoServico_Orcamento_OrcamentoId",
                table: "CustoServico",
                column: "OrcamentoId",
                principalTable: "Orcamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Peca_Orcamento_OrcamentoId",
                table: "Peca",
                column: "OrcamentoId",
                principalTable: "Orcamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoOrcamento_Orcamento_OrcamentoId",
                table: "ProdutoOrcamento",
                column: "OrcamentoId",
                principalTable: "Orcamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
