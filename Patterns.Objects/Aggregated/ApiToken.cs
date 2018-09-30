using System;

namespace Patterns.Objects.Aggregated
{
    public class ApiToken
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public ApiUser User { get; set; }
    }
}
