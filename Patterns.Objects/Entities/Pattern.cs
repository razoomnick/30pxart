using System;

namespace Patterns.Objects.Entities
{
    public class Pattern: DatabaseObject
    {
        public Pattern()
        {
            CreationTime = DateTime.UtcNow;
        }

        public string Name { get; set; }
        public PatternImage OriginalImage { get; set; }
        public PatternImage PatternImage { get; set; }
        public User Author { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDraft { get; set; }
        public string FilterName { get; set; }
    }
}
