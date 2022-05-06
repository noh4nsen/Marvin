using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Servicos.Commands
{
    public class InserirServicoCommand : IRequest<InserirServicoCommandResponse>
    {
        public string Nome { get; set; }
        public decimal? ValorAtual { get; set; }
    }

    public class InserirServicoCommandResponse
    {
        public Guid Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirServicoCommandHandler : IRequestHandler<InserirServicoCommand, InserirServicoCommandResponse>
    {
        private readonly IRepository<Servico> _repository;

        public InserirServicoCommandHandler(IRepository<Servico> repository)
        {
            _repository = repository;
        }

        public async Task<InserirServicoCommandResponse> Handle(InserirServicoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Servico entity = await AddServicoAsync(request);

            return entity.ToInsertResponse();
        }

        private async Task<Servico> AddServicoAsync(InserirServicoCommand request)
        {
            Servico entity = request.ToDomain();
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity;
        }
    }

    public class InserirServicoCommandValidator : AbstractValidator<InserirServicoCommand>
    {
        public InserirServicoCommandValidator()
        {
            RuleFor(r => r.Nome)
                .NotEmpty()
                .WithMessage("Nome do serviço não pode ser vazio.");
        }
    }
}
