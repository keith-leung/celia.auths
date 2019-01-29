using Celia.io.Core.Auths.Abstractions;
using Celia.io.Core.Auths.Abstractions.Exceptions;
using Celia.io.Core.MicroServices.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Celia.io.Core.Auths.Services
{
    public class ServiceAppAuthService : IServiceAppAuthService
    {
        private IServiceAppRepository _repository;
        private ILogger _logger;

        public ServiceAppAuthService(IServiceAppRepository repository, ILogger<ServiceAppAuthService> logger)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool CheckAuth(string appId, long timestamp, string path, string sign)
        {
            ServiceApp serviceApp = this._repository.GetServiceAppById(appId);
            if (serviceApp == null)
                return false;

            if (serviceApp.LockoutEnd != null && serviceApp.LockoutEnd.Value > DateTime.Now)
            {
                throw new AuthException($"The appid {appId} is locked. ", null)
                {
                    Code = (int)HttpStatusCode.NotAcceptable,
                };
            }

            string matchedSign = HashUtils.OpenApiSign(appId, serviceApp.AppSecret, timestamp, path);
            if (matchedSign.Equals(sign, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }
    }
}
