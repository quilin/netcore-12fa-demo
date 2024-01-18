using System.Reflection;
using TFA.API.Authentication;
using TFA.API.Middlewares;
using TFA.API.Monitoring;
using TFA.Domain.Authentication;
using TFA.Domain.DependencyInjection;
using TFA.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiLogging(builder.Configuration, builder.Environment)
    .AddApiMetrics(builder.Configuration);
builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication").Bind);
builder.Services.AddScoped<IAuthTokenStorage, AuthTokenStorage>();

builder.Services
    .AddForumDomain()
    .AddForumStorage(builder.Configuration.GetConnectionString("Postgres")!);
builder.Services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.MapPrometheusScrapingEndpoint();

app
    .UseMiddleware<ErrorHandlingMiddleware>()
    .UseMiddleware<AuthenticationMiddleware>();

app.Run();

public partial class Program
{
}