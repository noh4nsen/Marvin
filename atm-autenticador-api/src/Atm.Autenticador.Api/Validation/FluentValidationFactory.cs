using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atm.Autenticador.Api.Validation
{
    public class FluentValidationFactory : ValidatorFactoryBase
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _serviceProvider.GetService(validatorType) as IValidator;
        }
    }
}
