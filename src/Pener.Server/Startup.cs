using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Pener.Server.Services.Auth;
using Pener.Server.Services.Jwt;
using Pener.Services.User;
using Raven.DependencyInjection;

namespace Pener.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // use RavenDB
            services.Configure<RavenSettings>(Configuration.GetSection("RavenSettings"));
            services.AddRavenDbDocStore();
            services.AddRavenDbSession();
            services.AddRavenDbAsyncSession();

            // use ASP.NET Core MVC
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // use Response Compression.
            services.AddResponseCompression();

            // use Identity.
            services
                .AddIdentity<User, Role>(options => 
                {
                    // password setting.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddDefaultTokenProviders();

            // use Jwt Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme 
                    = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme 
                    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters 
                    = JwtService.GetValidationParameters(Configuration);
            });

            // use Logging
            services.AddLogging();

            // use Pener Services
            services.AddJwtService(Configuration);
            services.AddAuthService(Configuration);
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. 
                // You may want to change this for production scenarios, 
                // see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
