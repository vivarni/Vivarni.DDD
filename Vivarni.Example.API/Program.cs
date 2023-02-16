
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.Application;
using Vivarni.Example.Domain.Entities;
using Vivarni.Example.Infrastructure;
using Vivarni.Example.Shared.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapPost("/GuestMessages", async (IGenericRepository<GuestMessage> guestMessageRepository, GuestMessageCreateDTO insertModel) =>
{
    var entity = new GuestMessage(insertModel);
    return await guestMessageRepository.AddAsync(entity);
});
app.MapGet("/GuestMessages", async (IGenericRepository<GuestMessage> guestMessageRepository) =>
{
    var messages =  new List<GuestMessageDTO>();
    foreach(var message in await guestMessageRepository.ListAsync())
    {
        messages.Add(new GuestMessageDTO(message.Message, message.CreatedBy, message.CreationDate)); 
    }
    return messages;
});



app.Run();


