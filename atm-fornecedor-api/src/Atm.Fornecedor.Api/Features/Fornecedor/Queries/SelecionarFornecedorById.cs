using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Fornecedor.Queries
{
    public class SelecionarFornecedorByIdQuery : IRequest<SelecionarFornecedorQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class SelecionarFornecedorQueryResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class SelecionarFornecedorByIdQueryHandler : IRequestHandler<SelecionarFornecedorByIdQuery, SelecionarFornecedorQueryResponse>
    {
        private readonly IRepository<Domain.Fornecedor> _repository;
        private readonly SelecionarFornecedorByIdQueryValidator _validator;

        public SelecionarFornecedorByIdQueryHandler(IRepository<Domain.Fornecedor> repository, SelecionarFornecedorByIdQueryValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<SelecionarFornecedorQueryResponse> Handle(SelecionarFornecedorByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Domain.Fornecedor entity = await _repository.GetFirstAsync(f => f.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity);

            return entity.ToQueryResponse();
        }
    }

    public class SelecionarFornecedorByIdQueryValidator : AbstractValidator<SelecionarFornecedorByIdQuery>
    {
        public SelecionarFornecedorByIdQueryValidator()
        {
            RuleFor(f => f.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id inválido.");
        }

        public async Task ValidateDataAsync(SelecionarFornecedorByIdQuery request, Domain.Fornecedor entity)
        {
            RuleFor(f => f.Id)
                    .Must(m => { return entity != null; })
                    .WithMessage($"Fornecedor de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
