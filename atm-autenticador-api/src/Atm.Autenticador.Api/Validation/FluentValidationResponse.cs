using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace Atm.Autenticador.Api.Validation
{
    public class FluentValidationResponse
    {
        public string Title { get; set; }
        public int Status { get; set; }

        public FluentValidationResponse(ValidationException exception)
        {
            Status = GetStatus(exception);
            Title = GetTitle();
        }

        private int GetStatus(ValidationException exception)
        {
            if (exception.Message.Contains("não encontrado"))
                return StatusCodes.Status404NotFound;
            else
                return StatusCodes.Status400BadRequest;
        }

        private string GetTitle()
        {
            return Status switch
            {
                StatusCodes.Status404NotFound => "Objeto não encontrado",
                _ => "Operação inválida",
            };
        }
    }
}
