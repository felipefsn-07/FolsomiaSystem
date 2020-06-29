using AutoMapper;
using FolsomiaSystem.Application;
using FolsomiaSystem.Infra.IoC;
using FolsomiaSystem.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using static Microsoft.AspNetCore.Mvc.CompatibilityVersion;
using Newtonsoft.Json.Serialization;
using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using Prometheus;
using System.IO;
using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FolsomiaSystem.Api
{


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DefaultContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
         
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("SecurePassword")["TokenSecret"].ToString());
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            // Enable Health Checks
            services.AddHealthChecks();

            // Configuring HTTP Response Compression
            services.AddResponseCompression(
                options =>
                {
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });
            services.Configure<BrotliCompressionProviderOptions>(
                options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });
            services.Configure<GzipCompressionProviderOptions>(
                options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });

            services.AddApiVersioning(
                options =>
                {
                options.ReportApiVersions = true;
                options.Conventions.Add(new VersionByNamespaceConvention());

                    options.AssumeDefaultVersionWhenUnspecified = true;
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;

                    options.AssumeDefaultVersionWhenUnspecified = true;
                });

            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = true;
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .SetCompatibilityVersion(Latest);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Folsomia System API",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Felipe Silva do Nascimento",
                        Email = "felipefsn-07@hotmail.com",
                        Url = new Uri("https://www.github.com/felipefsn-07")
                    }
                });
                string caminhoAplicacao =
                PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc =
                    Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });
                c.OperationFilter<AuthOperationFilter>();

                c.IncludeXmlComments(caminhoXmlDoc);
            });


            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddAutoMapper(typeof(DomainMappingProfile).Assembly);
            services.AddDIConfiguration();
            services.AddHttpClient();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



  


            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                    options.RoutePrefix = string.Empty;
                });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
