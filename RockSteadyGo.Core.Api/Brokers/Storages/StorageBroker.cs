// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = configuration
                .GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        public override void Dispose() { }
    }
}
