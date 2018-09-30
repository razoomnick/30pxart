using System;

namespace Authentication.External
{
    public interface IAuthProvider
    {
        ExternalUser Authenticate(String code);
        String LoginLink { get; }
    }
}