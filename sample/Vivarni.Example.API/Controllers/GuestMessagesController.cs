using Microsoft.AspNetCore.Mvc;
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.API.ApiModels;
using Vivarni.Example.Domain.Entities;

namespace Vivarni.Example.API.Controllers;

[Route("api")]
[ApiController]
public class GuestMessagesController : ControllerBase
{
    private readonly IGenericRepository<GuestMessage> _guestMessageRepository;

    public GuestMessagesController(IGenericRepository<GuestMessage> repositoryBase)
    {
        _guestMessageRepository = repositoryBase;
    }

    [HttpGet("guestmessages")]
    public async Task<IActionResult> GetAllGuestMessages(CancellationToken cancellation)
    {
        var messages = await _guestMessageRepository.ListAsync(cancellation);
        var vms = messages.Select(message => new GuestMessageReadModel(message.Id, message.Message, message.CreatedBy, message.CreationDate));
        return Ok(vms);
    }

    [HttpGet("guestmessages/{id}")]
    public async Task<IActionResult> GetGuestMessageById(Guid id, CancellationToken cancellation)
    {
        var message = await _guestMessageRepository.GetByIdAsync<Guid>(id, cancellation);
        if (message == null)
            return NotFound();

        var vm = new GuestMessageReadModel(message.Id, message.Message, message.CreatedBy, message.CreationDate);
        return Ok(vm);
    }

    [HttpPost("guestmessages")]
    public async Task<IActionResult> PostGuestMessageWithDomainEvents([FromBody] GuestMessageWriteModel wm, CancellationToken cancellation)
    {
        var message = GuestMessage.GuestMessageCreate(wm.GuestMessage, wm.CreatedByUser);
        var vm = await _guestMessageRepository.AddAsync(message, cancellation);
        return Ok(vm);
    }

    [HttpDelete("guestmessages")]
    public async Task<IActionResult> DeleteGuestMessageById(Guid id, CancellationToken cancellation)
    {
        var message = await _guestMessageRepository.GetByIdAsync<Guid>(id, cancellation);
        if (message == null)
            return NotFound();

        await _guestMessageRepository.DeleteAsync(message, cancellation);
        return Ok();
    }
}
