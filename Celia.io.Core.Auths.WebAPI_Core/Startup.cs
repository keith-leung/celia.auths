using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Celia.io.Core.Auths.Abstractions; 
using Celia.io.Core.Auths.Services;
using Celia.io.Core.Auths.DataAccess.EfCore;
using Celia.io.Core.MicroServices.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
//using MySql.Data.MySqlClient;
using NLog.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Celia.io.Core.Auths.WebAPI_Core
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            //加入配置文件
            var builder = new ConfigurationBuilder()
               .SetBasePath(hostingEnvironment.ContentRootPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true);
            builder.AddEnvironmentVariables();

            Configuration = builder.Build();// configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(options =>
                { 
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(setupAction =>
                {
                    setupAction.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                });

            //太不稳定了，不加
            ////加入ASP.NET Core Elm 功能，记录所有HTTP请求信息
            //services.AddElm();

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddDefaultTokenProviders();

            services.AddAuthentication().AddJwtBearer();

            string connectionString = Configuration.GetConnectionString(
                "DefaultConnectionString");

            services.AddLogging(configure =>
            {
                configure.AddNLog();
            });

            // Identity Services
            services.AddTransient<IUserStore<ApplicationUser>, EfCoreUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, EfCoreRoleStore>();
 
            services.AddTransient<ApplicationUserManager>();
            services.AddTransient<ApplicationRoleManager>();
            services.AddTransient<ApplicationSignInManager>();
            services.AddTransient<UserClaimsPrincipalFactory<
                        ApplicationUser, ApplicationRole>, ApplicationUserClaimsPrincipalFactory>();

            services.AddTransient<IDbConnection>(
                    impl => new SqlConnection(connectionString)
                );
                //impl => new MySqlConnection(connectionString));

            SigningCredentials signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                    Configuration.GetValue<string>("SigningCredentials:Key"))),
                SecurityAlgorithms.HmacSha256Signature);
            services.AddSingleton<SigningCredentials>(signingCredentials);

            DisconfService disconf = new DisconfService(this.Configuration);
            disconf.CustomConfigs.Add("Issuer", Configuration.GetValue<string>("SigningCredentials:Issuer"));
            disconf.CustomConfigs.Add("Audience", Configuration.GetValue<string>("SigningCredentials:Audience"));

            services.AddSingleton<DisconfService>(disconf);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
            );
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseMySql(connectionString));

            services.AddLogging(configure =>
            {
                configure.AddNLog();
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //太不稳定了，不加
            ////加入ASP.NET Core Elm 功能，记录所有HTTP请求信息
            //app.UseElmPage();
            //app.UseElmCapture();

            //启用ASP.NET Core Identity
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
