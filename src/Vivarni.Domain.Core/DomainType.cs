namespace Vivarni.Domain.Core
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
    }
}
