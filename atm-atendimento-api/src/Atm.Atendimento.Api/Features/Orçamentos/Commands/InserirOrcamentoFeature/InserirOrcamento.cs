using Atm.Atendimento.Api.Extensions.Entities;
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

namespace Atm.Atendimento.Api.Features.Orçamentos.Commands.InserirOrcamentoFeature
{
    public class InserirOrcamentoCommandHandler : IRequestHandler<InserirOrcamentoCommand, InserirOrcamentoCommandResponse>
    {
        private readonly IRepository<Orcamento> _repository;
        private readonly IRepository<Servico> _repositoryServico;
        private readonly IClienteService _clienteService;
        private readonly IProdutoService _produtoService;
        private readonly InserirOrcamentoCommandValidator _validator;

        public InserirOrcamentoCommandHandler
            (
                IRepository<Orcamento> repository,
                IRepository<Servico> repositoryServico,
                IClienteService clienteService,
                IProdutoService produtoService,
                InserirOrcamentoCommandValidator validator
            )
        {
            _repository = repository;
            _repositoryServico = repositoryServico;
            _clienteService = clienteService;
            _produtoService = produtoService;
            _validator = validator;
        }

        public async Task<InserirOrcamentoCommandResponse> Handle(InserirOrcamentoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Orcamento entity = await SetupOrcamentoAsync(request, cancellationToken);
            await AddOrcamentoAsync(entity);

            return entity.ToInsertResponse();
        }

        private async Task AddOrcamentoAsync(Orcamento entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        private async Task<Orcamento> SetupOrcamentoAsync(InserirOrcamentoCommand request, CancellationToken cancellationToken)
        {
            ClienteOrcamento cliente = await GetClienteOrcamentoAsync(request, cancellationToken);
            CarroOrcamento carro = await GetCarroOrcamentoAsync(request, cancellationToken);
            IEnumerable<ProdutoOrcamento> produtos = await GetProdutoOrcamentosAsync(request, cancellationToken);
            IEnumerable<Peca> pecas = SetupPecas(request, cancellationToken);
            IEnumerable<CustoServico> servicos = await GetCustoServicosAsync(request, cancellationToken);
            Pagamento pagamento = SetupPagamento(request.Pagamento);

            Orcamento entity = request.ToDomain(cliente, carro, produtos, pecas, servicos, pagamento);
            return entity;
        }

        private async Task<ClienteOrcamento> GetClienteOrcamentoAsync(InserirOrcamentoCommand request, CancellationToken cancellationToken)
        {
            ClienteOrcamento entity = await _clienteService.GetClienteById(request.ClienteId);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity.ToNew();
        }

        private async Task<CarroOrcamento> GetCarroOrcamentoAsync(InserirOrcamentoCommand request, CancellationToken cancellationToken)
        {
            CarroOrcamento entity = await _clienteService.GetCarroById(request.CarroId);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity.ToNew();
        }

        private async Task<IEnumerable<ProdutoOrcamento>> GetProdutoOrcamentosAsync(InserirOrcamentoCommand request, CancellationToken cancellationToken)
        {
            if (request.Produtos.Count == 0 && !request.Produtos.Any())
                return new List<ProdutoOrcamento>();

            IList<ProdutoOrcamento> produtos = new List<ProdutoOrcamento>();
            foreach (var entity in request.Produtos)
            {
                ProdutoOrcamento produto = await GetProdutoOrcamentoAsync(request, entity.ProdutoId, cancellationToken);
                produtos.Add(entity.ToDomain(produto));
            }
            return produtos;
        }

        private async Task<ProdutoOrcamento> GetProdutoOrcamentoAsync(InserirOrcamentoCommand request, Guid id, CancellationToken cancellationToken)
        {
            ProdutoOrcamento entity = await _produtoService.GetProdutoById(id);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }

        private IEnumerable<Peca> SetupPecas(InserirOrcamentoCommand request, CancellationToken cancellationToken)
        {
            if (request.Pecas.Count == 0 && !request.Pecas.Any())
                return new List<Peca>();

            IList<Peca> pecas = new List<Peca>();
            foreach (var entity in request.Pecas)
            {
                pecas.Add(entity.ToDomain());
            }
            return pecas;
        }

        private async Task<IEnumerable<CustoServico>> GetCustoServicosAsync(InserirOrcamentoCommand request, CancellationToken cancellationToken)
        {
            if (request.Servicos.Count == 0 && !request.Servicos.Any())
                return new List<CustoServico>();

            IList<CustoServico> servicos = new List<CustoServico>();
            foreach (var entity in request.Servicos)
            {
                Servico servico = await GetServicoAsync(request, entity.ServicoId, cancellationToken);
                CustoServico custoServico = entity.ToDomain(servico);
                custoServico.Update(servico);
                servicos.Add(custoServico);
            }
            return servicos;
        }

        private async Task<Servico> GetServicoAsync(InserirOrcamentoCommand request, Guid id, CancellationToken cancellationToken)
        {
            Servico entity = await _repositoryServico.GetFirstAsync(s => s.Id.Equals(id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }

        private Pagamento SetupPagamento(InserirPagamentoCommand request)
        {
            Pagamento pagamento = request.ToDomain();
            return pagamento;
        }
    }
}
