using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace RestAPIConcepts.MiddlewareExtensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder Return422ForValidationErrors(this IMvcBuilder builder)
        {
            builder.ConfigureApiBehaviorOptions(setupAction =>
            {
                //We override the default behavior when a validation error occurs
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    // create a problem details object
                    var problemDetailsFactory = context.HttpContext.RequestServices
                         .GetRequiredService<ProblemDetailsFactory>();

                    var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                         context.HttpContext,
                         context.ModelState);

                    // add additional info not added by default
                    problemDetails.Detail = "See the errors field for details.";
                    problemDetails.Instance = context.HttpContext.Request.Path;

                    // find out which status code to use
                    var actionExecutingContext = context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    // if there are modelstate errors & all keys were correctly
                    // found/parsed we're dealing with validation errors
                    if ((context.ModelState.ErrorCount > 0) && 
                        (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                    {
                        problemDetails.Type = "https://RestApiConcepts.com/modelvalidationproblem";
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        problemDetails.Title = "One or more validation errors occurred.";

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    }

                    //ENSURE THAT WE STILL HANDLE 400 BAD REQUEST for BAD REQUEST THAT ROOTS FROM INVALID INPUT
                    // if one of the keys wasn't correctly found / couldn't be parsed
                    // we're dealing with null/unparsable input
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "One or more errors on input occurred.";
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

            return builder;
        }
    }
}
