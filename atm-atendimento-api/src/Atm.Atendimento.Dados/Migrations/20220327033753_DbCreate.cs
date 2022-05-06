using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Atm.Atendimento.Dados.Migrations
{
    public partial class DbCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarroOrcamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdExterno = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarroOrcamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClienteOrcamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdExterno = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteOrcamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModoPagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CartaoCredito = table.Column<bool>(type: "boolean", nullable: false),
                    CartaoDebito = table.Column<bool>(type: "boolean", nullable: false),
                    Dinheiro = table.Column<bool>(type: "boolean", nullable: false),
                    Pix = table.Column<bool>(type: "boolean", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModoPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ValorAtual = table.Column<decimal>(type: "numeric", nullable: true),
                    CustoServicoAtual = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Percentual = table.Column<decimal>(type: "numeric", nullable: true),
                    Desconto = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorFinal = table.Column<decimal>(type: "numeric", nullable: false),
                    PagamentoEfetuado = table.Column<bool>(type: "boolean", nullable: false),
                    ModoPagamentoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamento_ModoPagamento_ModoPagamentoId",
                        column: x => x.ModoPagamentoId,
                        principalTable: "ModoPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orcamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: true),
                    CarroId = table.Column<Guid>(type: "uuid", nullable: true),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PagamentoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataAgendamento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataHoraInicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataHoraFim = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Duracao = table.Column<double>(type: "double precision", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orcamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orcamento_CarroOrcamento_CarroId",
                        column: x => x.CarroId,
                        principalTable: "CarroOrcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orcamento_ClienteOrcamento_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "ClienteOrcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orcamento_Pagamento_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustoServico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ServicoId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrcamentoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustoServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustoServico_Orcamento_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalTable: "Orcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustoServico_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Peca",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ValorUnitario = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorCobrado = table.Column<decimal>(type: "numeric", nullable: false),
                    OrcamentoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peca_Orcamento_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalTable: "Orcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoOrcamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdExterno = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    Percentual = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    OrcamentoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoOrcamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoOrcamento_Orcamento_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalTable: "Orcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustoServico_OrcamentoId",
                table: "CustoServico",
                column: "OrcamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_CustoServico_ServicoId",
                table: "CustoServico",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Orcamento_CarroId",
                table: "Orcamento",
                column: "CarroId");

            migrationBuilder.CreateIndex(
                name: "IX_Orcamento_ClienteId",
                table: "Orcamento",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Orcamento_PagamentoId",
                table: "Orcamento",
                column: "PagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_ModoPagamentoId",
                table: "Pagamento",
                column: "ModoPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Peca_OrcamentoId",
                table: "Peca",
                column: "OrcamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoOrcamento_OrcamentoId",
                table: "ProdutoOrcamento",
                column: "OrcamentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustoServico");

            migrationBuilder.DropTable(
                name: "Peca");

            migrationBuilder.DropTable(
                name: "ProdutoOrcamento");

            migrationBuilder.DropTable(
                name: "Servico");

            migrationBuilder.DropTable(
                name: "Orcamento");

            migrationBuilder.DropTable(
                name: "CarroOrcamento");

            migrationBuilder.DropTable(
                name: "ClienteOrcamento");

            migrationBuilder.DropTable(
                name: "Pagamento");

            migrationBuilder.DropTable(
                name: "ModoPagamento");
        }
    }
}
