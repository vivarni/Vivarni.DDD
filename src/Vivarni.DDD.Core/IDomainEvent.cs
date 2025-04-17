namespace Vivarni.DDD.Core;

/// <summary>
/// An event is something that has happened in the past. A domain event is, something that
/// happened in the domain that you want other parts of the same domain (in-process) to be
/// aware of. The notified parts usually react somehow to the events. Domain events as a
/// preferred way to trigger side effects across multiple aggregates within the same domain.
/// </summary>
/// <seealso href="http://www.kamilgrzybek.com/design/how-to-publish-and-handle-domain-events/"/>
/// <seealso href="https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/"/>
public interface IDomainEvent { }
