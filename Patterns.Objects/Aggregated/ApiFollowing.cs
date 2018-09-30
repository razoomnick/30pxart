using System;

namespace Patterns.Objects.Aggregated
{
    public class ApiFollowing
    {
        public ApiUser Subscriber { get; set; }
        public ApiUser Publisher { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
