using System;
using System.Collections.Generic;
using System.Text;

namespace Celia.io.Core.Auths.Abstractions
{
    public interface IServiceAppAuthService
    {
        bool CheckAuth(string appId, long timestamp, string path, string sign);
    }
}
