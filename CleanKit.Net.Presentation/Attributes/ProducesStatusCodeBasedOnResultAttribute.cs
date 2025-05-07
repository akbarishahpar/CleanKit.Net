using System.Net;
using CleanKit.Net.Domain.Primitives.Error;
using CleanKit.Net.Domain.Primitives.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanKit.Net.Presentation.Attributes;

public class ProducesStatusCodeBasedOnResultAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult { Value: Result { IsFailure: true } result })
        {
            context.Result = result.Error switch
            {
                BadRequestError => new ObjectResult(result) { StatusCode = (int)HttpStatusCode.BadRequest },
                ConflictError => new ObjectResult(result) { StatusCode = (int)HttpStatusCode.Conflict },
                DependencyError => new ObjectResult(result) { StatusCode = (int)HttpStatusCode.FailedDependency },
                FinancialError => new ObjectResult(result) { StatusCode = (int)HttpStatusCode.PaymentRequired },
                ForbiddenError => new ObjectResult(result) { StatusCode = (int)HttpStatusCode.Forbidden },
                NotFoundError => new ObjectResult(result) { StatusCode = (int)HttpStatusCode.NotFound },
                ValidationError => new ObjectResult(result) { StatusCode = (int)HttpStatusCode.UnprocessableEntity },
                _ => context.Result
            };
        }
        base.OnResultExecuting(context);
    }
}