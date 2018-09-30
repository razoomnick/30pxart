using System;

namespace Patterns.Objects.Entities
{
    public class Like: DatabaseObject
    {
        public Like()
        {
            CreationTime = DateTime.UtcNow;
        }

        public User User { get; set; }
        public Pattern Pattern { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
