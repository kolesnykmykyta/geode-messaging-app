using DataAccess.DbContext;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;

namespace Geode.Api.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where
        TStartup : class
    {
        private const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TestGeodeDb;Integrated Security=true;";

        internal DatabaseContext DbContext { get; }

        private bool _disposed;

        public CustomWebApplicationFactory()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseSqlServer(ConnectionString);

            DbContext = new DatabaseContext(builder.Options);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                ServiceDescriptor? descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DatabaseContext>)
                );
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(ConnectionString);
                });
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                    base.Dispose(disposing);
                }
            }
            _disposed = true;
        }
    }
}
