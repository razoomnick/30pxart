using System;

namespace Patterns.Objects.Entities
{
    public class Following: DatabaseObject
    {
        public Following()
        {
            CreationTime = DateTime.UtcNow;
        }

        public User Subscriber { get; set; }
        public User Publisher { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
