using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Storage.v1;
using Patterns.Cloud.Common;

namespace Patterns.Cloud.Gcs
{
    public class GcsUploader: IUploader
    {
        private StorageService service = new StorageService();
        private readonly string bucketName = ConfigurationManager.AppSettings["GcsBucketName"];

        public GcsUploader(string rootPath)
        {
            var accountEmail = ConfigurationManager.AppSettings["GcsAccountEmail"];
            var password = ConfigurationManager.AppSettings["GcsPassword"];
            var keyPath = ConfigurationManager.AppSettings["GcsKeyPath"];
            var certificateFile = Path.Combine(rootPath, keyPath);
            var certificate = new X509Certificate2(certificateFile, password, 
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(accountEmail)
                {
                    Scopes = new[] { StorageService.Scope.DevstorageReadWrite }
                }.FromCertificate(certificate));
            service = new StorageService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });
        }

        public string Upload(string name, string contentType, byte[] data)
        {
            var result = "";
            try
            {
                var fileobj = new Google.Apis.Storage.v1.Data.Object()
                    {
                        Name = name,
                        Metadata = new Dictionary<string, string>()
                            {
                                {"Cache-Control", "public, max-age=2592000"}
                            }
                    };
                using (var stream = new MemoryStream(data))
                {
                    service.Objects.Insert(fileobj, bucketName, stream, contentType).Upload();
                }
                result = "http://storage.googleapis.com/" + bucketName + "/" + name;
            }
            catch(Exception ex){}
            return result;
        }
    }
}
