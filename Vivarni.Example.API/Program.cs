
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.Domain.Entities;
using Vivarni.Example.Infrastructure.SQLite;
using Vivarni.Example.Shared.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapPost("/GuestMessages", async (IGenericRepository<GuestMessage> guestMessageRepository, GuestMessageCreateDTO insertModel) =>
{
    var entity = new GuestMessage(insertModel);
    return await guestMessageRepository.AddAsync(entity);
});
app.MapGet("/GuestMessages", async (IGenericRepository<GuestMessage> guestMessageRepository) =>
{
    var messages = new List<GuestMessageDTO>();
    foreach (var message in await guestMessageRepository.ListAsync())
    {
        messages.Add(new GuestMessageDTO(message.Message, message.CreatedBy, message.CreationDate));
    }
    return messages;
});

app.Run();
