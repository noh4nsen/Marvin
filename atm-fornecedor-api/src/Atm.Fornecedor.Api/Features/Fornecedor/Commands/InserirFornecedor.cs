using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Fornecedor.Commands
{
    public class InserirFornecedorCommand : IRequest<InserirFornecedorCommandResponse>
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public string Endereco { get; set; }
    }

    public class InserirFornecedorCommandResponse
    {
        public Guid Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirFornecedorCommandHandler : IRequestHandler<InserirFornecedorCommand, InserirFornecedorCommandResponse>
    {
        private readonly IRepository<Domain.Fornecedor> _repository;

        public InserirFornecedorCommandHandler(IRepository<Domain.Fornecedor> repository)
        {
            _repository = repository;
        }

        public async Task<InserirFornecedorCommandResponse> Handle(InserirFornecedorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Domain.Fornecedor entity = request.ToDomain();
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return entity.ToInsertResponse();
        }
    }

    public class InserirFornecedorCommandValidator : AbstractValidator<InserirFornecedorCommand>
    {
        public InserirFornecedorCommandValidator()
        {
            RuleFor(f => f.Nome).NotEmpty()
                                .WithMessage("Nome de fornecedor é obrigatório");
        }
    }
}
