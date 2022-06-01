using Conduit.API.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApiControllers
{
    public static IServiceCollection AddConduitControlles(this IServiceCollection services)
    {
        services.AddControllers(option =>
            {
                option.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                option.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                option.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                option.ReturnHttpNotAcceptable = true;
                option.Filters.Add(new AuthorizeFilter());
                option.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .ConfigureApiBehaviorOptions(opt =>
                {
                    opt.InvalidModelStateResponseFactory = context =>
                    {
                        var type = context.GetType();
                        Console.WriteLine(type);
                        var problemFactory =
                            context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                        var problemDetails =
                            problemFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

                        problemDetails.Detail = "See errors fields for more details";
                        problemDetails.Instance = context.HttpContext.Request.Path;

                        var actionExecutingContext = context as ActionExecutingContext;
                        if (context.ModelState.ErrorCount > 0 &&
                            (actionExecutingContext?.ActionArguments.Count ==
                             context.ActionDescriptor.Parameters.Count ||
                             (actionExecutingContext == null && context.GetType() == typeof(ControllerContext) &&
                              !context.ModelState.IsValid))
                           )
                        {
                            problemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
                            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                            problemDetails.Title = "One or more validation errors occurred.";

                            return new UnprocessableEntityObjectResult(problemDetails)
                            {
                                ContentTypes = { "application/problem+json" }
                            };
                        }

                        // if one of the keys wasn't correctly found / couldn't be parsed
                        // we're dealing with null/unparsable input
                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Title = "One or more errors on input occurred.";
                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                }
            ).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining(typeof(UserEditingValidator));
                options.RegisterValidatorsFromAssemblyContaining(typeof(ArticleCreatingValidator));
                options.RegisterValidatorsFromAssemblyContaining(typeof(CommentCreationValidator));
            });

        return services;
    }
}