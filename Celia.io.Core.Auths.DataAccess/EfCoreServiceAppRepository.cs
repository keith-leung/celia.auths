using Celia.io.Core.Auths.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Celia.io.Core.Auths.DataAccess.EfCore
{
    public class EfCoreServiceAppRepository : IServiceAppRepository
    {
        private readonly ApplicationDbContext _context = null;
        private readonly ILogger logger = null;

        public EfCoreServiceAppRepository(ILogger<EfCoreServiceAppRepository> logger, 
            ApplicationDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(
                nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        } 

        /// <summary>
        /// 不能通过内存实现加速，加速使用缓存是Service层的事情
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public Task<ServiceApp> GetByAppIdAppSecretAsync(string appId, string appSecret)
        {
            return Task.Run<ServiceApp>(() =>
            { 
                var serviceApp = _context.ServiceApps.FirstOrDefault(m => m.AppId.Equals(
                appId, StringComparison.InvariantCultureIgnoreCase)
                && m.AppSecret.Equals(appSecret, StringComparison.InvariantCultureIgnoreCase)); 

                return serviceApp;
            });
        }

        public Task UpdateAccessFailedCountAsync(string appId, string appSecret, int count)
        {
            return Task.Run(() =>
            {
                var serviceApp = _context.ServiceApps.FirstOrDefault(m => m.AppId.Equals(
                    appId, StringComparison.InvariantCultureIgnoreCase)
                    && m.AppSecret.Equals(appSecret, StringComparison.InvariantCultureIgnoreCase));

                if (serviceApp != null)
                {
                    serviceApp.AccessFailedCount = count;
                    _context.SaveChanges();
                } 
            });
        }

        public Task UpdateLockoutEnabled(string appId, string appSecret, bool enabled)
        {
            return Task.Run(() =>
            {
                var serviceApp = _context.ServiceApps.FirstOrDefault(m => m.AppId.Equals(
                    appId, StringComparison.InvariantCultureIgnoreCase)
                    && m.AppSecret.Equals(appSecret, StringComparison.InvariantCultureIgnoreCase));

                if (serviceApp != null)
                {
                    serviceApp.LockoutEnabled = enabled;
                    _context.SaveChanges();
                } 
            });
        }

        public Task UpdateLockoutEnd(string appId, string appSecret, DateTimeOffset? lockoutEnd)
        {
            return Task.Run(() =>
            {
                var serviceApp = _context.ServiceApps.FirstOrDefault(m => m.AppId.Equals(
                    appId, StringComparison.InvariantCultureIgnoreCase)
                    && m.AppSecret.Equals(appSecret, StringComparison.InvariantCultureIgnoreCase));

                if (serviceApp != null)
                {
                    serviceApp.LockoutEnd = lockoutEnd;
                    _context.SaveChanges();
                } 
            });
        }

        public ServiceApp GetServiceAppById(string appId)
        {
            return _context.ServiceApps.Find(appId);
        }
    }
}
