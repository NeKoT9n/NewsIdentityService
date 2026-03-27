using Microsoft.AspNetCore.Mvc;
using NewsApi.Domain.Common.Validation;

namespace IdentityService.Extiensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<TResult>(this Result<TResult, Error> result)
    {
        if (result.IsFailure)
        {
            return new BadRequestObjectResult(result.Error);
        }
        
        return new OkObjectResult(result.Value);
    }
    
}