using Ardalis.Specification;
using Microsoft.AspNetCore.Mvc;
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.API.ApiModels;
using Vivarni.Example.Domain.Entities;

namespace Vivarni.Example.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestMessagesWithDomainEventsController : ControllerBase
    {
        private readonly IGenericRepository<GuestMessage> _guestMessageRepository;

        public GuestMessagesWithDomainEventsController(IGenericRepository<GuestMessage> repositoryBase)
        {
            _guestMessageRepository = repositoryBase;
        }
        [HttpPost("GuestMessagesWithDomainEvents")]
        public async Task<IActionResult> PostGuestMessageWithDomainEvents([FromBody] GuestMessageWriteModel wm, CancellationToken cancellation)
        {
            var entity = new GuestMessage(wm.GuestMessage, wm.CreatedByUser);
            entity.Events.Add(new GuestMessageCreatedEvent(entity));
            return Ok(await _guestMessageRepository.AddAsync(entity, cancellation));
        }
    }
}
