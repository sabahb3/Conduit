using System.Text;
using Conduit.Data;
using Conduit.Data.IRepositories;
using Conduit.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;

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
}
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ConduitDbContext>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
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