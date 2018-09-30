using System;

namespace Patterns.Objects.Entities
{
    public class Comment: DatabaseObject
    {
        public Comment()
        {
            CreationTime = DateTime.UtcNow;
        }

        public User Author { get; set; }
        public Pattern Pattern { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
