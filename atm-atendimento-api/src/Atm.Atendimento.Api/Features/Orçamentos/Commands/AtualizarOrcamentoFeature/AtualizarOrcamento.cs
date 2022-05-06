using Atm.Atendimento.Api.Helpers;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Dto;
using Atm.Atendimento.Repositories;
using Atm.Atendimento.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Commands.AtualizarOrcamentoFeature
{
    public class AtualizarOrcamentoCommandHandler : IRequestHandler<AtualizarOrcamentoCommand, AtualizarOrcamentoCommandResponse>
    {
        private readonly IRepository<Orcamento> _repository;
        private readonly IRepository<ClienteOrcamento> _repositoryCliente;
        private readonly IRepository<CarroOrcamento> _repositoryCarro;
        private readonly IRepository<Servico> _repositoryServico;
        private readonly IRepository<ProdutoOrcamento> _repositoryProduto;
        private readonly IRepository<Peca> _repositoryPeca;
        private readonly IRepository<CustoServico> _repositoryCustoServico;
        private readonly IRepository<Pagamento> _repositoryPagamento;
        private readonly IRepository<ModoPagamento> _repositoryModoPagamento;
        private readonly IClienteService _clienteService;
        private readonly IProdutoService _produtoService;
        private readonly AtualizarOrcamentoCommandValidator _validator;

        public AtualizarOrcamentoCommandHandler
            (
                IRepository<Orcamento> repository,
                IRepository<ClienteOrcamento> repositoryCliente,
                IRepository<CarroOrcamento> repositoryCarro,
                IRepository<Servico> repositoryServico,
                IRepository<ProdutoOrcamento> repositoryProduto,
                IRepository<Peca> repositoryPeca,
                IRepository<CustoServico> repositoryCustoServico,
                IRepository<Pagamento> repositoryPagamento,
                IRepository<ModoPagamento> repositoryModoPagamento,
                IClienteService clienteService,
                IProdutoService produtoService,
                AtualizarOrcamentoCommandValidator validator
            )
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _repositoryCarro = repositoryCarro;
            _repositoryServico = repositoryServico;
            _repositoryProduto = repositoryProduto;
            _repositoryPeca = repositoryPeca;
            _repositoryCustoServico = repositoryCustoServico;
            _repositoryPagamento = repositoryPagamento;
            _repositoryModoPagamento = repositoryModoPagamento;
            _repositoryPeca = repositoryPeca;
            _clienteService = clienteService;
            _produtoService = produtoService;
            _validator = validator;
        }

        public async Task<AtualizarOrcamentoCommandResponse> Handle(AtualizarOrcamentoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Orcamento entity = await GetOrcamentoAsync(request, cancellationToken);
            await UpdateOrcamentoAsync(request, entity, cancellationToken);

            return new AtualizarOrcamentoCommandResponse() { DataAtualizacao = entity.DataAtualizacao };
        }

        #region Orcamento
        private async Task UpdateOrcamentoAsync(AtualizarOrcamentoCommand request, Orcamento entity, CancellationToken cancellationToken)
        {
            entity.Cliente = await UpdateClienteOrcamentoAsync(request, entity.Cliente, cancellationToken);
            entity.Carro = await UpdateCarroOrcamentoAsync(request, entity.Carro, cancellationToken);
            entity.Produtos = (await MergeProdutoOrcamentosAsync(request, entity, cancellationToken)).ToList();
            entity.Pecas = (await MergePecasAsync(request, entity, cancellationToken)).ToList();
            entity.CustoServicos = (await MergeCustoServicosAsync(request, entity, cancellationToken)).ToList();
            entity.Pagamento = await UpdatePagamentoAsync(request, entity, cancellationToken);
            entity.Descricao = request.Descricao;
            entity.DataAtualizacao = DateHelper.GetLocalTime();

            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        private async Task<Orcamento> GetOrcamentoAsync(AtualizarOrcamentoCommand request, CancellationToken cancellationToken)
        {
            Orcamento entity = await _repository.GetFirstAsync(o => o.Id.Equals(request.Id),
                                                               o => o.Cliente,
                                                               o => o.Carro,
                                                               o => o.Produtos,
                                                               o => o.CustoServicos,
                                                               o => o.Pagamento,
                                                               o => o.Pecas);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
        #endregion Orcamento

        #region ClienteOrcamento
        private async Task<ClienteOrcamento> UpdateClienteOrcamentoAsync(AtualizarOrcamentoCommand request, ClienteOrcamento cliente, CancellationToken cancellationToken)
        {
            ClienteOrcamento entity = await _clienteService.GetClienteById(request.ClienteId);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);

            cliente.IdExterno = entity.IdExterno;
            cliente.DataAtualizacao = DateHelper.GetLocalTime();

            await _repositoryCliente.UpdateAsync(cliente);
            return cliente;
        }
        #endregion

        #region CarroOrcamento
        private async Task<CarroOrcamento> UpdateCarroOrcamentoAsync(AtualizarOrcamentoCommand request, CarroOrcamento carro, CancellationToken cancellationToken)
        {
            CarroOrcamento entity = await _clienteService.GetCarroById(request.CarroId);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);

            carro.IdExterno = entity.IdExterno;
            carro.DataAtualizacao = DateHelper.GetLocalTime();

            await _repositoryCarro.UpdateAsync(carro);
            return carro;
        }
        #endregion Carro

        #region ProdutoOrcamento
        private async Task<IEnumerable<ProdutoOrcamento>> MergeProdutoOrcamentosAsync(AtualizarOrcamentoCommand request, Orcamento entity, CancellationToken cancellationToken)
        {
            IEnumerable<ProdutoOrcamento> listaNovos = await UpdateProdutoOrcamentosAsync(request, entity, cancellationToken);
            foreach (var produto in entity.Produtos)
            {
                if (!listaNovos.ToList().Exists(l => l.Id.Equals(produto.Id)))
                    await RemoveProdutoOrcamentoAsync(produto);
            }
            return listaNovos;
        }
        private async Task<IEnumerable<ProdutoOrcamento>> UpdateProdutoOrcamentosAsync(AtualizarOrcamentoCommand request, Orcamento entity, CancellationToken cancellationToken)
        {
            if (!request.Produtos.Any())
                return new List<ProdutoOrcamento>();

            IList<ProdutoOrcamento> listaNovos = new List<ProdutoOrcamento>();
            foreach (var produto in request.Produtos)
            {
                if (produto.Id is null)
                    listaNovos.Add(await NewProdutoOrcamento(request, produto, entity, cancellationToken));
                else
                    listaNovos.Add(await UpdateProdutoOrcamentoAsync(request, produto, cancellationToken));
            }
            return listaNovos;
        }

        private async Task<ProdutoOrcamento> NewProdutoOrcamento(AtualizarOrcamentoCommand request, AtualizarProdutoCommand produto, Orcamento orcamento, CancellationToken cancellationToken)
        {
            ProdutoOrcamento entity = await _produtoService.GetProdutoById(produto.ProdutoId);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);

            entity.Id = Guid.NewGuid();
            entity.Ativo = true;
            entity.Quantidade = produto.Quantidade;
            entity.ValorUnitario = produto.ValorUnitario;
            entity.Percentual = produto.Percentual;
            entity.ValorTotal = produto.ValorTotal;
            entity.Orcamento = orcamento;
            entity.DataCadastro = DateHelper.GetLocalTime();
            
            await _repositoryProduto.AddAsync(entity);
            return entity;
        }

        private async Task<ProdutoOrcamento> UpdateProdutoOrcamentoAsync(AtualizarOrcamentoCommand request, AtualizarProdutoCommand produto, CancellationToken cancellationToken)
        {
            ProdutoOrcamento validador = await _produtoService.GetProdutoById(produto.ProdutoId);
            await _validator.ValidateDataAsync(request, validador, cancellationToken);

            ProdutoOrcamento entity = await GetProdutoOrcamentoAsync(request, (Guid)produto.Id, cancellationToken);
            entity.IdExterno = validador.IdExterno;
            entity.Quantidade = produto.Quantidade;
            entity.ValorUnitario = produto.ValorUnitario;
            entity.Percentual = produto.Percentual;
            entity.ValorTotal = produto.ValorTotal;
            entity.DataAtualizacao = DateHelper.GetLocalTime();

            await _repositoryProduto.UpdateAsync(entity);
            return entity;
        }

        private async Task RemoveProdutoOrcamentoAsync(ProdutoOrcamento entity)
        {
            await _repositoryProduto.RemoveAsync(entity);
        }

        private async Task<ProdutoOrcamento> GetProdutoOrcamentoAsync(AtualizarOrcamentoCommand request, Guid id, CancellationToken cancellationToken)
        {
            ProdutoOrcamento entity = await _repositoryProduto.GetFirstAsync(cs => cs.Id.Equals(id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
        #endregion ProdutoOrcamento

        #region Peca
        private async Task<IEnumerable<Peca>> MergePecasAsync(AtualizarOrcamentoCommand request, Orcamento entity, CancellationToken cancellationToken)
        {
            IEnumerable<Peca> listaNovos = await UpdatePecasAsync(request, entity, cancellationToken);
            foreach (var peca in entity.Pecas)
            {
                if (!listaNovos.ToList().Exists(l => l.Id.Equals(peca.Id)))
                    await RemovePecaAsync(peca);
            }
            return listaNovos;
        }

        private async Task<IEnumerable<Peca>> UpdatePecasAsync(AtualizarOrcamentoCommand request, Orcamento entity, CancellationToken cancellationToken)
        {
            if (!request.Pecas.Any())
                return new List<Peca>();

            IList<Peca> listaNovos = new List<Peca>();
            foreach (var peca in request.Pecas)
            {
                if (peca.Id is null)
                    listaNovos.Add(await NewPeca(peca, entity));
                else
                    listaNovos.Add(await UpdatePecaAsync(request, peca, cancellationToken));
            }
            return listaNovos;
        }

        private async Task<Peca> NewPeca(AtualizarPecaCommand peca, Orcamento orcamento)
        {
            Peca entity = new Peca()
                            {
                                Id = Guid.NewGuid(),
                                Orcamento = orcamento,
                                CodigoNCM = peca.CodigoNCM,
                                Nome = peca.Nome,
                                Descricao = peca.Descricao,
                                ValorUnitarioCompra = peca.ValorUnitarioCompra,
                                ValorUnitarioVenda = peca.ValorUnitarioVenda,
                                Quantidade = peca.Quantidade,
                                Percentual = peca.Percentual,
                                ValorCobrado = peca.ValorCobrado,
                                DataCadastro = DateHelper.GetLocalTime()
                            };
            await _repositoryPeca.AddAsync(entity);
            return entity;
        }

        private async Task<Peca> UpdatePecaAsync(AtualizarOrcamentoCommand request, AtualizarPecaCommand peca, CancellationToken cancellationToken)
        {
            Peca entity = await GetPecaAsync(request, (Guid)peca.Id, cancellationToken);

            entity.CodigoNCM = peca.CodigoNCM;
            entity.Nome = peca.Nome;
            entity.Descricao = peca.Descricao;
            entity.ValorUnitarioCompra = peca.ValorUnitarioCompra;
            entity.ValorUnitarioVenda = peca.ValorUnitarioVenda;
            entity.Quantidade = peca.Quantidade;
            entity.Percentual = peca.Percentual;
            entity.ValorCobrado = peca.ValorCobrado;
            entity.DataAtualizacao = DateHelper.GetLocalTime();

            await _repositoryPeca.UpdateAsync(entity);
            return entity;
        }

        private async Task RemovePecaAsync(Peca entity)
        {
            await _repositoryPeca.RemoveAsync(entity);
        }

        private async Task<Peca> GetPecaAsync(AtualizarOrcamentoCommand request, Guid id, CancellationToken cancellationToken)
        {
            Peca entity = await _repositoryPeca.GetFirstAsync(cs => cs.Id.Equals(id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
        #endregion Peca

        #region CustoServico
        private async Task<IEnumerable<CustoServico>> MergeCustoServicosAsync(AtualizarOrcamentoCommand request, Orcamento entity, CancellationToken cancellationToken)
        {
            IEnumerable<CustoServico> listaNovos = await UpdateCustoServicosAsync(request, entity, cancellationToken);
            foreach (var custoServico in entity.CustoServicos)
            {
                if (!listaNovos.ToList().Exists(l => l.Id.Equals(custoServico.Id)))
                    await RemoveCustoServicoAsync(custoServico);
            }
            return listaNovos;
        }

        private async Task<IEnumerable<CustoServico>> UpdateCustoServicosAsync(AtualizarOrcamentoCommand request, Orcamento entity, CancellationToken cancellationToken)
        {
            if (!request.Servicos.Any())
                return new List<CustoServico>();

            IList<CustoServico> listaNovos = new List<CustoServico>();
            foreach (var custoServico in request.Servicos)
            {
                if (custoServico.Id is null)
                    listaNovos.Add(await NewCustoServicoAsync(request, custoServico, entity, cancellationToken));
                else
                    listaNovos.Add(await UpdateCustoServicoAsync(request, custoServico, cancellationToken));
            }

            return listaNovos;
        }

        private async Task<CustoServico> UpdateCustoServicoAsync(AtualizarOrcamentoCommand request, AtualizarCustoServicoCommand custoServico, CancellationToken cancellationToken)
        {
            CustoServico entity = await GetCustoServicoAsync(request, (Guid)custoServico.Id, cancellationToken);

            entity.Servico = await GetServicoAsync(request, custoServico.ServicoId, cancellationToken);
            entity.Valor = custoServico.Valor;
            entity.Descricao = custoServico.Descricao;
            entity.DataAtualizacao = DateHelper.GetLocalTime();

            await _repositoryCustoServico.UpdateAsync(entity);
            return entity;
        }

        private async Task<CustoServico> NewCustoServicoAsync(AtualizarOrcamentoCommand request, AtualizarCustoServicoCommand custoServico, Orcamento orcamento, CancellationToken cancellationToken)
        {
            CustoServico entity = new CustoServico()
            {
                Id = Guid.NewGuid(),
                Ativo = true,
                Servico = await GetServicoAsync(request, custoServico.ServicoId, cancellationToken),
                Orcamento = orcamento,
                Valor = custoServico.Valor,
                Descricao = custoServico.Descricao,
                DataCadastro = DateHelper.GetLocalTime()
            };
            await _repositoryCustoServico.AddAsync(entity);
            return entity;
        }

        private async Task RemoveCustoServicoAsync(CustoServico entity)
        {
            await _repositoryCustoServico.RemoveAsync(entity);
        }

        private async Task<CustoServico> GetCustoServicoAsync(AtualizarOrcamentoCommand request, Guid id, CancellationToken cancellationToken)
        {
            CustoServico entity = await _repositoryCustoServico.GetFirstAsync(cs => cs.Id.Equals(id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }

        private async Task<Servico> GetServicoAsync(AtualizarOrcamentoCommand request, Guid id, CancellationToken cancellationToken)
        {
            Servico entity = await _repositoryServico.GetFirstAsync(cs => cs.Id.Equals(id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
        #endregion CustoServico

        #region Pagamento
        private async Task<Pagamento> UpdatePagamentoAsync(AtualizarOrcamentoCommand request, Orcamento orcamento, CancellationToken cancellationToken)
        {
            Pagamento entity = await GetPagamentoAsync(request, orcamento.Pagamento.Id, cancellationToken);

            entity.Percentual = request.Pagamento.Percentual;
            entity.Desconto = request.Pagamento.Desconto;
            entity.ValorFinal = request.Pagamento.ValorFinal;
            entity.PagamentoEfetuado = request.Pagamento.PagamentoEfetuado;
            entity.ModoPagamento = await UpdateModoPagamentoAsync(request, cancellationToken);
            entity.DataAtualizacao = DateHelper.GetLocalTime();

            await _repositoryPagamento.UpdateAsync(entity);
            return entity;
        }

        private async Task<Pagamento> GetPagamentoAsync(AtualizarOrcamentoCommand request, Guid id, CancellationToken cancellationToken)
        {
            Pagamento entity = await _repositoryPagamento.GetFirstAsync(p => p.Id.Equals(id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }

        private async Task<ModoPagamento> UpdateModoPagamentoAsync(AtualizarOrcamentoCommand request, CancellationToken cancellationToken)
        {
            ModoPagamento entity = await GetModoPagamentoAsync(request, request.Pagamento.ModoPagamento.Id, cancellationToken);

            entity.CartaoCredito = request.Pagamento.ModoPagamento.CartaoCredito;
            entity.CartaoDebito = request.Pagamento.ModoPagamento.CartaoDebito;
            entity.Dinheiro = request.Pagamento.ModoPagamento.Dinheiro;
            entity.Pix = request.Pagamento.ModoPagamento.Pix;
            entity.DataAtualizacao = DateHelper.GetLocalTime();

            await _repositoryModoPagamento.UpdateAsync(entity);
            return entity;
        }

        private async Task<ModoPagamento> GetModoPagamentoAsync(AtualizarOrcamentoCommand request, Guid id, CancellationToken cancellationToken)
        {
            ModoPagamento entity = await _repositoryModoPagamento.GetFirstAsync(p => p.Id.Equals(id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
        #endregion Pagamento
    }
}
