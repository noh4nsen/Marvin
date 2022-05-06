using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Fornecedor.Commands
{
    public class AtualizarFornecedorCommand : IRequest<AtualizarFornecedorCommandResponse>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public string Endereco { get; set; }
    }

    public class AtualizarFornecedorCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarFornecedorCommandHandler : IRequestHandler<AtualizarFornecedorCommand, AtualizarFornecedorCommandResponse>
    {
        private readonly IRepository<Domain.Fornecedor> _repository;
        private readonly AtualizarFornecedorCommandValidator _validator;

        public AtualizarFornecedorCommandHandler(IRepository<Domain.Fornecedor> repository, AtualizarFornecedorCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<AtualizarFornecedorCommandResponse> Handle(AtualizarFornecedorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Domain.Fornecedor entity = await _repository.GetFirstAsync(f => f.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity);

            request.Update(entity);

            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();

            return entity.ToUpdateResponse();
        }
    }

    public class AtualizarFornecedorCommandValidator : AbstractValidator<AtualizarFornecedorCommand>
    {
        public AtualizarFornecedorCommandValidator()
        {
            RuleFor(f => f.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de fornecedor é obrigatório");
            RuleFor(f => f.Nome)
                .NotEmpty()
                .WithMessage("Nome de fornecedor é obrigatório");
        }

        public async Task ValidateDataAsync(AtualizarFornecedorCommand request, Domain.Fornecedor entity)
        {
            RuleFor(r => r.Id)
                .Must(f => { return entity != null; })
                .WithMessage($"Fornecedor de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
