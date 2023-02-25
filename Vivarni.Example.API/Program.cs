
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.API.ApiModels;
using Vivarni.Example.Domain.Entities;
using Vivarni.Example.Infrastructure.SQLite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapPost("/GuestMessages", async (IGenericRepository<GuestMessage> guestMessageRepository, GuestMessageWriteModel wm) =>
{
    var entity = new GuestMessage(wm.GuestMessage, wm.CreatedByUser);
    return await guestMessageRepository.AddAsync(entity);
});
app.MapGet("/GuestMessages", async (IGenericRepository<GuestMessage> guestMessageRepository) =>
{
    var messages = new List<GuestMessageReadModel>();
    foreach (var message in await guestMessageRepository.ListAsync())
    {
        messages.Add(new GuestMessageReadModel(message.Message, message.CreatedBy, message.CreationDate));
    }
    return messages;
});

app.Run();
