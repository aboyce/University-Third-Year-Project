using System.Data.Common;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagement.Models.Context;
using File = System.IO.File;

namespace TicketManagement.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        private ApplicationContext _db;
        private string currentFile;

        public ApplicationContext Database => _db;

        [TestInitialize]
        public void Initialise()
        {
            currentFile = Path.GetTempFileName();

            if (string.IsNullOrEmpty(currentFile))
                return;

            string connectionString = $"Data Source={currentFile};Persist Security Info=False";

            DbConnection connection = DbProviderFactories.GetFactory("System.Data.SqlServerCe.4.0").CreateConnection();

            if(connection == null)
                return;

            connection.ConnectionString = connectionString;
            connection.Open();

            _db = new ApplicationContext(connection);
            _db.Database.CreateIfNotExists();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _db.Dispose();
            _db = null;
            File.Delete(currentFile);
        }

        protected abstract void SeedDatabase();
    }
}
