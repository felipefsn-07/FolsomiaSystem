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

           /* services.AddDbContext<DefaultContext>(options => options
               .UseInMemoryDatabase("DefaultContext")
                .UseLazyLoadingProxies());*/


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

            //services.AddDbContext<DefaultContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddApiVersioning(
                options =>
                {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;

                // automatically applies an api version based on the name of the defining controller's namespace
                options.Conventions.Add(new VersionByNamespaceConvention());

                    options.AssumeDefaultVersionWhenUnspecified = true;
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
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
            // Register the Swagger generator, defining 1 or more Swagger documents
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

            app.UseAuthorization();

            app.UseCors(option => option.AllowAnyOrigin());
            //app.UseAuthentication();
            //app.UseHealthChecks();
            //app.UseMetricServer();
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
