using FunPokedex.Business;
using FunPokedex.PokemonApi;
using FunPokedex.ShakespeareApi;
using FunPokemon.YodaApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace FunPokedex.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IPokemonApiService, PokemonApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["Services:PokemonApi:BaseUri"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler(ExponentialBackoffRetryPolicy());

            services.AddHttpClient<IYodaApiService, YodaApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["Services:YodaApi:BaseUri"]);
            })
            .AddPolicyHandler(ExponentialBackoffRetryPolicy());

            services.AddHttpClient<IShakespeareApiService, ShakespeareApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["Services:ShakespeareeApi:BaseUri"]);
            })
            .AddPolicyHandler(ExponentialBackoffRetryPolicy());

            services.AddTransient<IPokemonService, PokemonService>();
            services.AddMemoryCache();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FunPokedex.Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // For production, this would usually sit in the above block and be disabled, however for demo purposes we need it
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FunPokedex.Api v1"));

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> ExponentialBackoffRetryPolicy()
        {
            Random jitterer = new();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(6,    // exponential back-off plus some jitter
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                  + TimeSpan.FromMilliseconds(jitterer.Next(0, 100))
                );
        }
    }
}
