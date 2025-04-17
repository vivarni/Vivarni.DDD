using Vivarni.Example.Domain;
using Vivarni.Example.Infrastructure.SQLite;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddInfrastructureServices(builder.Configuration)
    .AddDomainServices()
    .AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
app.MapControllers();
await app.RunAsync();
