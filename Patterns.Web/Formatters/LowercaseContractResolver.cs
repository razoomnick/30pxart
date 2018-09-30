using Newtonsoft.Json.Serialization;

namespace Patterns.Web.Formatters
{
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            string result = propertyName;
            if (propertyName.Length > 0)
            {
                result = propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
            }
            return result;
        }
    }
}