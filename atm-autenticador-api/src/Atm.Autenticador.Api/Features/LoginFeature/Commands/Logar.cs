using Atm.Autenticador.Api.Helpers;
using Atm.Autenticador.Domain;
using Atm.Autenticador.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Autenticador.Api.Features.LoginFeature.Commands
{
    public class LogarCommand : IRequest<LogarCommandResponse>
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class LogarCommandResponse
    {
        public string Token { get; set; }
    }

    public class LogarCommandHandler : IRequestHandler<LogarCommand, LogarCommandResponse>
    {
        private readonly IRepository<Usuario> _repository;
        private readonly LogarCommandValidator _validator;

        public LogarCommandHandler(IRepository<Usuario> repository, LogarCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<LogarCommandResponse> Handle(LogarCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Usuario entity = await LogarAsync(request, cancellationToken);
            entity = await RenewJwtAsync(entity);

            return new LogarCommandResponse() { Token = entity.Token };
        }

        private async Task<Usuario> RenewJwtAsync(Usuario entity)
        {
            string token = CryptographyHelper.RenewJwt(entity.Id, entity.Token);
            if (token.Equals(entity.Token))
                return entity;
            else
            {
                entity.Token = token;
                await _repository.UpdateAsync(entity);
                await _repository.SaveChangesAsync();
                return entity;
            }
        }

        private async Task<Usuario> LogarAsync(LogarCommand request, CancellationToken cancellationToken)
        {
            Usuario entity = await _repository.GetFirstAsync(u => u.Login.Equals(request.Login) && u.Senha.Equals(CryptographyHelper.ToHash(request.Senha)));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
    }

    public class LogarCommandValidator : AbstractValidator<LogarCommand>
    {
        public LogarCommandValidator()
        {
            RuleFor(l => l.Login)
                .NotEmpty()
                .WithMessage("Login é obrigatório.");
            RuleFor(l => l.Senha)
                .NotEmpty()
                .WithMessage("Senha é obrigatória.");
        }

        public async Task ValidateDataAsync
            (
                LogarCommand request,
                Usuario entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Login)
                .Must(u => { return entity is not null; })
                .WithMessage($"Login {request.Login} inválido.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
