using System;

namespace Patterns.Objects.Entities
{
    public class DatabaseObject
    {
        public DatabaseObject()
        {
            Id = Guid.NewGuid();
        }
        public virtual Guid Id { get; set; }
    }
}