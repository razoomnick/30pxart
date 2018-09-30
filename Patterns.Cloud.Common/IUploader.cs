namespace Patterns.Cloud.Common
{
    public interface IUploader
    {
        string Upload(string name, string contentType, byte[] data);
    }
}
