using Microsoft.AspNetCore.Mvc;
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.API.ApiModels;
using Vivarni.Example.Domain.Entities;

namespace Vivarni.Example.API.Controllers;

[Route("api")]
[ApiController]
public class GuestMessagesCounterController : ControllerBase
{
    private readonly IGenericRepository<GuestMessagesCounter> _counterRepository;
    public GuestMessagesCounterController(IGenericRepository<GuestMessagesCounter> counterRepository)
    {
        _counterRepository = counterRepository;
    }

    [HttpGet("guestmessagescounter")]
    public async Task<IActionResult> GetCounters(CancellationToken cancellation)
    {
        var counters = await _counterRepository.ListAsync(cancellation);
        var counter = counters.SingleOrDefault();
        if (counter == null)
            return Ok("The domain event hasn't been called yet. Create a new message for the datamigration to take place");

        var vm = new GuestMessageCounterReadModel(counter.Count, counter.LastEntryBy);
        return Ok(vm);
    }
}
