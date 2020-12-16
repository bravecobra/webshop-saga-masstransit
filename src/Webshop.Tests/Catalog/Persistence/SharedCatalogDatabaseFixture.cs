using System;
using System.Data.Common;
using Catalog.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Webshop.Tests.Catalog.Persistence
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SharedCatalogDatabaseFixture : IDisposable
    {
        private static readonly object Lock = new object();
        private static bool _databaseInitialized;
        private string Connectionstring { get; set; }

        private DbConnection Connection { get; set; }

        public void InitializeNewDatabase(string connectionstring)
        {
            Connectionstring = connectionstring;
            Connection = new SqlConnection(connectionstring);
            Seed();
            Connection.Open();
        }

        public CatalogDbContext CreateContext(ILoggerFactory loggerFactory = null, DbTransaction transaction = null )
        {
            var context = new CatalogDbContext(new DbContextOptionsBuilder<CatalogDbContext>()
                .UseLoggerFactory(loggerFactory)
                .UseSqlServer(Connectionstring).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (Lock)
            {
                if (_databaseInitialized) return;
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }

                _databaseInitialized = true;
            }
        }

        public void Dispose() => Connection.Dispose();
    }
}