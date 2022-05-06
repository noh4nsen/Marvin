using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature
{
    public class SelecionarOrcamentoByIdHandler : IRequestHandler<SelecionarOrcamentoByIdQuery, SelecionarOrcamentoByIdQueryResponse>
    {
        private readonly IRepository<Orcamento> _repository;
        private readonly IRepository<CustoServico> _repositoryCustoServico;
        private readonly IRepository<Pagamento> _repositoryPagamento;
        private readonly SelecionarOrcamentoByIdQueryValidator _validator;

        public SelecionarOrcamentoByIdHandler
            (
                IRepository<Orcamento> repository, 
                IRepository<CustoServico> repositoryCustoServico, 
                IRepository<Pagamento> repositoryPagamento,
                SelecionarOrcamentoByIdQueryValidator validator
            )
        {
            _repository = repository;
            _repositoryCustoServico = repositoryCustoServico;
            _repositoryPagamento = repositoryPagamento;
            _validator = validator;
        }

        public async Task<SelecionarOrcamentoByIdQueryResponse> Handle(SelecionarOrcamentoByIdQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Orcamento entity = await SetupOrcamentoAsync(request, cancellationToken);
            return entity.ToQueryResponse();
        }

        private async Task<Orcamento> SetupOrcamentoAsync(SelecionarOrcamentoByIdQuery request, CancellationToken cancellationToken)
        {
            Orcamento entity = await GetOrcamentoAsync(request, cancellationToken);
            IEnumerable<CustoServico> custoServicos = await GetCustoServicosAsync(request, entity, cancellationToken);
            Pagamento pagamento = await GetPagamentoAsync(request, entity, cancellationToken);
            entity.CustoServicos = custoServicos.ToList();
            entity.Pagamento = pagamento;
            return entity;
        }

        private async Task<Orcamento> GetOrcamentoAsync(SelecionarOrcamentoByIdQuery request, CancellationToken cancellationToken)
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

        private async Task<IEnumerable<CustoServico>> GetCustoServicosAsync(SelecionarOrcamentoByIdQuery request, Orcamento entity, CancellationToken cancellationToken)
        {
            IEnumerable<CustoServico> custoServicos = await _repositoryCustoServico.GetAsync(cs => cs.Orcamento.Id.Equals(entity.Id), cs => cs.Servico);
            foreach (var custoServico in custoServicos)
                await _validator.ValidateDataAsync(request, custoServico, cancellationToken);
            return custoServicos;
        }

        private async Task<Pagamento> GetPagamentoAsync(SelecionarOrcamentoByIdQuery request, Orcamento entity, CancellationToken cancellationToken)
        {
            Pagamento pagamento = await _repositoryPagamento.GetFirstAsync(p => p.Id.Equals(entity.Pagamento.Id), p => p.ModoPagamento);
            await _validator.ValidateDataAsync(request, pagamento, cancellationToken);
            return pagamento;
        }
    }
}
