using System;

namespace Patterns.Objects.Aggregated
{
    public class ApiComment
    {
        public Guid Id { get; set; }
        public ApiUser Author { get; set; }
        public Guid PatternId { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime{ get; set; }
    }
}
