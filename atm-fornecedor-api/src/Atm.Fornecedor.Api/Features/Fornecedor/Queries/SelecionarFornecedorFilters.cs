using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Fornecedor.Queries
{
    public class SelecionarFornecedorFiltersQuery : IRequest<IEnumerable<SelecionarFornecedorQueryResponse>>
    {
        public string Nome { get; set; }
    }

    public class SelecionarFornecedorFiltersQueryHandler : IRequestHandler<SelecionarFornecedorFiltersQuery, IEnumerable<SelecionarFornecedorQueryResponse>>
    {
        private readonly IRepository<Domain.Fornecedor> _repository;

        public SelecionarFornecedorFiltersQueryHandler(IRepository<Domain.Fornecedor> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarFornecedorQueryResponse>> Handle(SelecionarFornecedorFiltersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            IEnumerable<Domain.Fornecedor> result = string.IsNullOrEmpty(request.Nome) 
                                                ? await _repository.GetAsync() 
                                                : await _repository.GetAsync(f => f.Nome.ToUpper().Contains(request.Nome.ToUpper()));

            return result.ToQueryResponse();
        }
    }
}
