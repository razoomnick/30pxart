using System;

namespace Patterns.Objects.Entities
{
    public class User: DatabaseObject
    {
        public User()
        {
            RegistrationTime = DateTime.UtcNow;
        }
        
        public string ExternalId { get; set; }
        public string Provider { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationTime { get; set; }
        public PatternImage Avatar { get; set; }
    }
}