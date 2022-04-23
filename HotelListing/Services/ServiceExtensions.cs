using AspNetCoreRateLimit;
using HotelListing.Data;
using HotelListing.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace HotelListing.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        { 
            var builder = services.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtSettings = Configuration.GetSection("Jwt");

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.GetSection("Audience").Value,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("Key").Value))
                    //security Issue sensitive data by adding the key to jwt token in appsettings file(dont have admin rights)
                };
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error => {
                error.Run(async context => {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        Log.Error($"Something went wrong in the {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorModel
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Please try again later."
                        }.ToString());
                    }
                });
            });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1,0);//default version of api when requests are made
                option.ApiVersionReader = new HeaderApiVersionReader("api-version"); //used to get the api version
                                                                                     //from the header of the request
                                                                                     //for not altering the uri 
            });
        }

        //making cacheing globally
        //it is still possible to override these settings in the controller if we need another spec
        public static void ConfigureHttpCacheHeader(this IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddHttpCacheHeaders(
                expirationOpt =>
                {
                    expirationOpt.MaxAge = 120;
                    expirationOpt.CacheLocation = CacheLocation.Private;
                },
                validationOpt =>
                {
                    validationOpt.MustRevalidate = true;
                }
                );
        }

        //adding limits to protect from ddos or just limiting the calls from a specific client if he tries to overflow the server
        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint= "POST:/api/Account/Login",    //to a specific endpoint(for example on login if someone enters wrong credentials more than 5 times in a minute
                    Limit= 5,          //5 calls per 1 minute etc etc etc
                    Period = "1m"
                },
                ////we can add more Rules;
                new RateLimitRule
                {
                    Endpoint = "*",     //all endpoints
                    Limit = 5,         //limited to 1 call
                    Period = "1s"      //per second
                }
            };

            services.Configure<IpRateLimitOptions>(op =>
            {
                op.GeneralRules = rateLimitRules;
                op.EnableEndpointRateLimiting = true;
            });

            //specific for library
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            //for new version of ratelimit 4.0.1 we need also this
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();
        }
    }
}
