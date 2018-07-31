using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Newtonsoft.Json.Serialization;

namespace dotnet2_1WebAPI
{
    public partial class Startup
    {
        public SymmetricSecurityKey signingKey;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appsettingbuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var Configuration = appsettingbuilder.Build();         
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<AppDb>(options =>
                options.UseMySql(connectionString));

            // Add framework services.
            // Add OutputFormatters ContentType at service, it can define at Util/HtmlOutputFormatter.cs (eg., text/html)
            services
            .AddMvc(options => options.OutputFormatters.Add(new HtmlOutputFormatter())) 
            .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(30);
            });

            // ********************
            // Setup CORS
            // ********************
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsAllowAllPolicy", corsBuilder.Build());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwt =>
            {
                
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                    // Validate the token expiry
                    ValidateLifetime = true,
                    // If you want to allow a certain amount of clock drift, set that here:
                    ClockSkew = TimeSpan.Zero
                };
                string key = "JFTUFICUCQ4GS3MI6RBM39SD"; //TODO read data from config
                double expiretimespan = Convert.ToDouble(Configuration.GetSection("TokenAuthentication:TokenExpire").Value);
            
                /*jwt.SecurityTokenValidators.Clear();
                jwt.SecurityTokenValidators.Add(new OAuthTokenHandler(expiretimespan,key));*/
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();   
            //app.UseStaticFiles(); // For the wwwroot folder
            //app.UseS; // For the wwwroot folder
            // Serve my app-specific default file, if present.
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options); 
            app.UseStaticFiles();
            //app.UseStaticFiles("");  
            //app.UsePathBase("/videocvadmin");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All,
                RequireHeaderSymmetry = false,
                ForwardLimit = null,
                KnownProxies = { System.Net.IPAddress.Parse("172.31.130.61"), System.Net.IPAddress.Parse("127.0.0.1") },
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors("CorsAllowAllPolicy");

            app.UseAuthentication();
            double expiretimespan = Convert.ToDouble(Configuration.GetSection("TokenAuthentication:TokenExpire").Value);
            TimeSpan expiration = TimeSpan.FromMinutes(expiretimespan);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));
            /*var tokenProviderOptions = new TokenProviderOptions
            {
                Path = Configuration.GetSection("TokenAuthentication:TokenPath").Value,
                Audience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                Issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity,
                Expiration = expiration
            };
            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(tokenProviderOptions));*/

            loggerFactory.AddConsole(LogLevel.Warning, true).AddDebug();

            app.UseMvcWithDefaultRoute();

            using (var serviceScope = app.ApplicationServices.CreateScope())            
            {                
                var DB = serviceScope.ServiceProvider.GetService<AppDb>();// Seed the database.            
            }

            //var DB = app.ApplicationServices.GetRequiredService<AppDb>();
            //DB.Database.EnsureCreated();
        }
        
        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            
            return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
        }
    }
}
