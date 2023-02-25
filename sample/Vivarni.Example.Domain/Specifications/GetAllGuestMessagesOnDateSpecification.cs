using Ardalis.Specification;
using Vivarni.Example.Domain.Entities;

namespace Vivarni.Example.Application.Specifications;

public class GetAllGuestMessagesOnDateSpecification : Specification<GuestMessage>
{
    public GetAllGuestMessagesOnDateSpecification(DateTime date)
    {
        Query.Where(x => x.CreationDate == date);
    }
}
