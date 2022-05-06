using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Autenticador.Api.Validation
{
    public class FluentValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public FluentValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).ToList();
            if (failures.Any())
                throw new ValidationException("Requisição inválida.", failures);
            ClearFields(request);
            return await next();
        }

        private void ClearFields(TRequest request)
        {
            var props = request.GetType().GetProperties();
            foreach (var prop in props)
                if (prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(request);
                    string cleared = value == null || string.IsNullOrEmpty(value as string) ? string.Empty : value.ToString().Trim();
                    prop.SetValue(request, cleared);
                }
        }
    }
}
