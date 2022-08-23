namespace Vivarni.DDD.Core
{
    /// <summary>
    /// Base class for enum-like classes which can be extended with domain bahaviour/logic.
    /// See the docs for examples.
    /// </summary>
    public abstract class DomainType
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

        /// <summary />
        public static bool operator !=(DomainType lhs, DomainType rhs) => !(lhs == rhs);

        /// <summary />
        public static bool operator ==(DomainType lhs, DomainType rhs)
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
            return lhs.Name.Equals(rhs.Name);
        }
    }
}
