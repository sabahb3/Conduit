using System.Reflection;
using System.Text;
using Conduit.API.Helper;
using Conduit.API.Validators;
using Conduit.Data;
using Conduit.Data.IRepositories;
using Conduit.Data.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddControllers(option=>
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
        opt.InvalidModelStateResponseFactory = (context) =>
        {
            var type = context.GetType();
            Console.WriteLine(type);
            var problemFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = problemFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

            problemDetails.Detail = "See errors fields for more details";
            problemDetails.Instance = context.HttpContext.Request.Path;

            var actionExecutingContext = context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
            if ((context.ModelState.ErrorCount > 0) &&
                ((actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count)||
                 (actionExecutingContext == null && context.GetType()==typeof(ControllerContext) && !context.ModelState.IsValid))
               )
            {
                problemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                problemDetails.Title = "One or more validation errors occurred.";

                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes = {"application/problem+json"}
                };
            }

            // if one of the keys wasn't correctly found / couldn't be parsed
            // we're dealing with null/unparsable input
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "One or more errors on input occurred.";
            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = {"application/problem+json"}
            };
        };
    }
    
    ).AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining(typeof(UserEditingValidator));
    options.RegisterValidatorsFromAssemblyContaining(typeof(ArticleCreatingValidator));
    options.RegisterValidatorsFromAssemblyContaining(typeof(CommentCreationValidator));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    setupAction =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory,xmlFile);
        setupAction.IncludeXmlComments(xmlPath);
    }
    );
builder.Services.AddDbContext<ConduitDbContext>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IArticleRepository, ArticleRepository>();
builder.Services.AddTransient<ITagRepository, TagRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient(typeof(UserIdentity));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();