using System.Data.SqlClient;
using System.Text;

using Dapper;
using FluentMigrator.Runner;
using Jason.WebApi.Migrations.Interfaces;

namespace Jason.WebApi.Migrations.Services
{
    // Basic docs on how to this are here:
    // https://fluentmigrator.github.io/articles/quickstart.html?tabs=runner-in-process
    public class MigratorService : IMigratorService
    {
        private IMigrationRunner runner;
        private IConfiguration cfg;

        public MigratorService(IMigrationRunner runner, IConfiguration cfg)
        {
            this.runner = runner;
            this.cfg = cfg;
        }

        public string MigrateUp()
        {
            EnsureDatabase();

            var errs = ConsoleHook(() => runner.MigrateUp());
            var result = String.IsNullOrEmpty(errs) ? "Success" : errs;

            return result;
        }

        // Migrate down *to* the version.
        // If you want to migrate down the first migration, use any version # prior to that first migration.
        public string MigrateDown(long version)
        {
            var errs = ConsoleHook(() => runner.MigrateDown(version));
            var result = String.IsNullOrEmpty(errs) ? "Success" : errs;

            return result;
        }

        private void EnsureDatabase()
        {
            var cs = cfg.GetConnectionString(AppSettings.Settings.UseDatabase);
            var dbName = cs.RightOf("Database=").LeftOf(";");
            var master = cfg.GetConnectionString("MasterConnection");

            var parameters = new DynamicParameters();
            parameters.Add("name", dbName);
            using var connection = new SqlConnection(master);
            var records = connection.Query("SELECT name FROM sys.databases WHERE name = @name", parameters);

            if (!records.Any())
            {
                connection.Execute($"CREATE DATABASE [{dbName}]");
            }
        }

        private string ConsoleHook(Action action)
        {
            var saved = Console.Out;
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            Console.SetOut(tw);

            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            tw.Close();

            // Restore the default console out.
            // Simpler: https://stackoverflow.com/a/26095640
            Console.SetOut(saved);

            var errs = sb.ToString();

            return errs;
        }
    }
}
