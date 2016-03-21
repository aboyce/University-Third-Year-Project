using System.Data.Common;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagement.Models.Context;

namespace TicketManagement.Tests
{
    [TestClass]
    public class TestBase
    {
        private ApplicationContext db;
        private string currentFile;

        protected virtual void Seed() { }

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

            db = new ApplicationContext(connection);
            db.Database.CreateIfNotExists();

            Seed();
        }

        [TestCleanup]
        public void Cleanup()
        {
            db.Dispose();
            db = null;
            File.Delete(currentFile);
        }
    }
}
