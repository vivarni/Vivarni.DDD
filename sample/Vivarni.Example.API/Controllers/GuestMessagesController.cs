using Microsoft.AspNetCore.Mvc;
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.API.ApiModels;
using Vivarni.Example.Domain.Entities;

namespace Vivarni.Example.API.Controllers;

[Route("api")]
[ApiController]
public class GuestMessagesController(IGenericRepository<GuestMessage> repositoryBase) : ControllerBase
{

    [HttpGet("guestmessages")]
    public async Task<IActionResult> GetAllGuestMessages(CancellationToken cancellation)
    {
        var messages = await repositoryBase.ListAsync(cancellation);
        var vms = messages.Select(message => new GuestMessageReadModel(message.Id, message.Message, message.CreatedBy, message.CreationDate));
        return Ok(vms);
    }

    [HttpGet("guestmessages/{id}")]
    public async Task<IActionResult> GetGuestMessageById(Guid id, CancellationToken cancellation)
    {
        var message = await repositoryBase.GetByIdAsync<Guid>(id, cancellation);
        if (message == null)
            return NotFound();

        var vm = new GuestMessageReadModel(message.Id, message.Message, message.CreatedBy, message.CreationDate);
        return Ok(vm);
    }

    [HttpPost("guestmessages")]
    public async Task<IActionResult> PostGuestMessageWithDomainEvents([FromBody] GuestMessageWriteModel wm, CancellationToken cancellation)
    {
        var message = GuestMessage.GuestMessageCreate(wm.GuestMessage, wm.CreatedByUser);
        var vm = await repositoryBase.AddAsync(message, cancellation);
        return Ok(vm);
    }

    [HttpDelete("guestmessages")]
    public async Task<IActionResult> DeleteGuestMessageById(Guid id, CancellationToken cancellation)
    {
        var message = await repositoryBase.GetByIdAsync<Guid>(id, cancellation);
        if (message == null)
            return NotFound();

        await repositoryBase.DeleteAsync(message, cancellation);
        return Ok();
    }
}
