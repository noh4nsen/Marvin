using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Servicos.Queries
{
    public class SelecionarServicoByIdQuery : IRequest<SelecionarServicoByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class SelecionarServicoByIdQueryResponse
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public string Nome { get; set; }
        public decimal? ValorAtual { get; set; }
    }

    public class SelecionarServicoByIdQueryHandler : IRequestHandler<SelecionarServicoByIdQuery, SelecionarServicoByIdQueryResponse>
    {
        private readonly IRepository<Servico> _repository;
        private readonly SelecionarServicoByIdQueryValidator _validator;

        public SelecionarServicoByIdQueryHandler(IRepository<Servico> repository, SelecionarServicoByIdQueryValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<SelecionarServicoByIdQueryResponse> Handle(SelecionarServicoByIdQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Servico entity = await GetServicoAsync(request, cancellationToken);

            return entity.ToQueryResponse();
        }

        public async Task<Servico> GetServicoAsync(SelecionarServicoByIdQuery request, CancellationToken cancellationToken)
        {
            Servico entity = await _repository.GetFirstAsync(s => s.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
    }

    public class SelecionarServicoByIdQueryValidator : AbstractValidator<SelecionarServicoByIdQuery>
    {
        public SelecionarServicoByIdQueryValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de serviço é obrigatório.");
        }

        public async Task ValidateDataAsync(SelecionarServicoByIdQuery request, Servico entity, CancellationToken cancellationToken)
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Serviço de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
