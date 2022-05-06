using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Atm.Autenticador.Api.Validation
{
    public class FluentValidationExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = -10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                var details = GetDetails(exception);
                context.Result = GetResult(details);
                context.ExceptionHandled = true;
            }
        }

        private IActionResult GetResult(ValidationProblemDetails details)
        {
            return details.Status switch
            {
                StatusCodes.Status404NotFound => new NotFoundObjectResult(details),
                _ => new BadRequestObjectResult(details),
            };
        }

        private ValidationProblemDetails GetDetails(ValidationException exception)
        {
            var errors = exception.Errors
                                  .GroupBy(x => x.PropertyName, x => x.ToString())
                                  .ToDictionary(x => x.Key, x => x.ToArray());
            var response = new FluentValidationResponse(exception);
            return new ValidationProblemDetails(errors)
            {
                Title = response.Title,
                Status = response.Status
            };
        }
    }
}
