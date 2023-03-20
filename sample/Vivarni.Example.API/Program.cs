
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.API.ApiModels;
using Vivarni.Example.Domain.Entities;
using Vivarni.Example.Infrastructure.SQLite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapPost("/minimal/api/GuestMessages", async (IGenericRepository<GuestMessage> guestMessageRepository, GuestMessageWriteModel wm, CancellationToken cancellation) =>
{
    var entity = new GuestMessage(wm.GuestMessage, wm.CreatedByUser);
    return await guestMessageRepository.AddAsync(entity, cancellation);
}).WithTags("GuestMessages");

app.MapGet("/minimal/api/GuestMessages", async (IGenericRepository<GuestMessage> guestMessageRepository, CancellationToken cancellation) =>
{
    var messages = new List<GuestMessageReadModel>();
    foreach (var message in await guestMessageRepository.ListAsync(cancellation))
    {
        messages.Add(new GuestMessageReadModel(message.Id, message.Message, message.CreatedBy, message.CreationDate));
    }
    return messages;
}).WithTags("GuestMessages");

app.MapDelete("/minimal/api/GuestMessages", async (IGenericRepository<GuestMessage> guestMessageRepository, Guid id, CancellationToken cancellation) =>
{
    var entityToDelete = await guestMessageRepository.GetByIdAsync(id, cancellation);
    if (entityToDelete != null)
    {
        await guestMessageRepository.DeleteAsync(entityToDelete, cancellation);
    }
}).WithTags("GuestMessages");

app.MapGet("/minimal/api/counter", async (IGenericRepository<GuestMessagesCounter> guestMessagesCounterRepository, CancellationToken cancellation) =>
{
    return await guestMessagesCounterRepository.ListAsync(cancellation);
});

app.MapControllers();

app.Run();
