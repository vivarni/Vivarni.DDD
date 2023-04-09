﻿using Microsoft.AspNetCore.Mvc;
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.API.ApiModels;
using Vivarni.Example.Domain.Entities;
using System.Linq;

namespace Vivarni.Example.API.Controllers;

[Route("api/[controller]")]
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
        List<GuestMessageReadModel> messages = ((await _guestMessageRepository.ListAsync(cancellation))
            .Select(message => new GuestMessageReadModel(message.Id, message.Message, message.CreatedBy, message.CreationDate)))
            .ToList();
        return Ok(messages);
    }
    [HttpGet("guestmessages/{id}")]
    public async Task<IActionResult> GetGuestMessageById([FromBody] Guid id, CancellationToken cancellation)
    {
        GuestMessage? message = await _guestMessageRepository.GetByIdAsync<Guid>(id, cancellation);
        if (message == null)
            return NotFound();
        return Ok(new GuestMessageReadModel(message.Id, message.Message, message.CreatedBy, message.CreationDate));
    }
    [HttpPost("guestmessages")]
    public async Task<IActionResult> PostGuestMessageWithDomainEvents([FromBody] GuestMessageWriteModel wm, CancellationToken cancellation)
    {
        GuestMessage message = new GuestMessage(wm.GuestMessage, wm.CreatedByUser);
        message.Events.Add(new GuestMessageCreatedEvent(message));
        return Ok(await _guestMessageRepository.AddAsync(message, cancellation));
    }
    [HttpDelete("guestmessages")]
    public async Task<IActionResult> DeleteGuestMessageById([FromBody] Guid id, CancellationToken cancellation)
    {
        GuestMessage? message = await _guestMessageRepository.GetByIdAsync<Guid>(id, cancellation);
        if (message == null)
            return NotFound();
        await _guestMessageRepository.DeleteAsync(message, cancellation);
        return Ok();
    }

}
