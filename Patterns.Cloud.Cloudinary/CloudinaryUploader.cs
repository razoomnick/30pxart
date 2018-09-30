using System;
using System.Configuration;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Patterns.Cloud.Common;

namespace Patterns.Cloud.Cloudinary
{
    public class CloudinaryUploader: IUploader
    {
        private readonly CloudinaryDotNet.Cloudinary cloudinary;

        public CloudinaryUploader()
        {
            var cloudName = ConfigurationManager.AppSettings["CloudinaryCloudName"];
            var apiKey = ConfigurationManager.AppSettings["CloudinaryApiKey"];
            var apiSecret = ConfigurationManager.AppSettings["CloudinaryApiSecret"];
            var account = new Account(cloudName, apiKey, apiSecret);
            cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public string Upload(string name, string contentType, byte[] data)
        {
            string result = "";
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(name, new MemoryStream(data)),
                    PublicId = name
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                result = uploadResult.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
