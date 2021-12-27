namespace Vivarni.Domain.Core
{
    /// <summary>
    /// Marker interface to mark aggregate root entities.
    /// 
    /// Aggregates draw a boundary around one or more Entities. An Aggregate enforces invariants
    /// for all its Entities for any operation it supports.  Each Aggregate has a Root Entity,
    /// which is the only member of the Aggregate that any object outside the Aggregate is allowed
    /// to hold a reference to.  From Evans, the rules we need to enforce include:
    /// 
    ///  * The root Entity has global identity and is ultimately responsible for checking invariants
    ///  * Root Entities have global identity.  Entities inside the boundary have local identity, unique only within the Aggregate.
    ///  * Nothing outside the Aggregate boundary can hold a reference to anything inside, except to the root Entity.  The root Entity can hand references to the internal Entities to other objects, but they can only use them transiently (within a single method or block).
    ///  * Only Aggregate Roots can be obtained directly with database queries.  Everything else must be done through traversal.
    ///  * Objects within the Aggregate can hold references to other Aggregate roots.
    ///  * A delete operation must remove everything within the Aggregate boundary all at once
    ///  * When a change to any object within the Aggregate boundary is committed, all invariants of the whole Aggregate must be satisfied.
    ///  
    /// </summary>
    /// <see href="https://lostechies.com/jimmybogard/2008/05/21/entities-value-objects-aggregates-and-roots/"/>
    public interface IAggregateRoot { }
}
