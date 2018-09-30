namespace Patterns.Objects.Entities
{
    public class PatternImage: DatabaseObject
    {
        public string ContentType { get; set; }
        public byte[] RawData { get; set; }
        public string CdnUrl { get; set; }
    }
}
