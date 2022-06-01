using System.Reflection;
using Conduit.API.Helper;
using Conduit.Data;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddConduitControlles();
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
builder.Services.AddWebRepositories();
builder.Services.AddTransient(typeof(UserIdentity));

var multiplexer = ConnectionMultiplexer.Connect("localhost");
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
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