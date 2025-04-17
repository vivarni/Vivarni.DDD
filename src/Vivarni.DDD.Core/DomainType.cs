using System;
using System.Collections.Generic;

namespace Vivarni.DDD.Core;

/// <summary>
/// Base class for enum-like classes which can be extended with domain bahaviour/logic.
/// See the docs for examples.
/// </summary>
public abstract class DomainType : IEqualityComparer<DomainType>, IEquatable<DomainType>
{
    /// <summary>
    /// Type name, as stored in the database.
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Create a new instance of this class.
    /// </summary>
    protected DomainType(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Returns the name of this type.
    /// </summary>
    public override string ToString()
    {
        return Name;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) => this.Equals(obj as DomainType);

    /// <inheritdoc/>
    public bool Equals(DomainType? p)
    {
        if (p is null)
        {
            return false;
        }

        // Optimization for a common success case.
        if (Object.ReferenceEquals(this, p))
        {
            return true;
        }

        // If run-time types are not exactly the same, return false.
        if (this.GetType() != p.GetType())
        {
            return false;
        }

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        return (Name == p.Name);
    }

    /// <inheritdoc/>
    public bool Equals(DomainType x, DomainType y) => x.Equals(y);

    /// <inheritdoc/>
    public int GetHashCode(DomainType obj) => obj.GetHashCode();

    /// <summary />
    public static bool operator !=(DomainType? lhs, DomainType? rhs) => !(lhs == rhs);

    /// <summary />
    public static bool operator ==(DomainType? lhs, DomainType? rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                // null == null = true.
                return true;
            }

            // Only the left side is null.
            return false;
        }
        // Equals handles the case of null on right side.
        return lhs.Equals(rhs);
    }
}
