using FluentValidation.Results;
using System.Collections.Generic;

namespace Core.Extensions.Exception
{
    public class ValidationErrorDetails : ErrorDetails
    {
        public IEnumerable<ValidationFailure> ValidationErrors { get; set; }
    }
}
