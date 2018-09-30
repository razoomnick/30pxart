using System;

namespace Patterns.Objects.Entities
{
    public class Token: DatabaseObject
    {
        public Token()
        {
            CreationTime = DateTime.UtcNow;
            ExpirationTime = DateTime.UtcNow.AddDays(31);
        }

        public User User { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
