// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RockSteadyGo.Core.Api.Brokers.DateTimes;
using RockSteadyGo.Core.Api.Brokers.Loggings;
using RockSteadyGo.Core.Api.Brokers.Storages;
using RockSteadyGo.Core.Api.Services.Foundations.Matches;
using RockSteadyGo.Core.Api.Services.Foundations.Moves;
using RockSteadyGo.Core.Api.Services.Foundations.Players;

namespace RockSteadyGo.Core.Api
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
            services.AddLogging();

            services.AddControllers()
                .AddOData(options => options.Select().Filter().Expand().OrderBy().Count().SetMaxTop(25));

            services.AddDbContext<StorageBroker>();
            AddBrokers(services);
            AddServices(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RockSteadyGo.Core.Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RockSteadyGo.Core.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IMatchService, MatchService>();
            services.AddTransient<IMoveService, MoveService>();
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
        }
    }
}
