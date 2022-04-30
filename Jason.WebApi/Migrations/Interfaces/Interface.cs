namespace Jason.WebApi.Migrations.Interfaces
{
    public interface IMigratorService
    {
        string MigrateUp();
        string MigrateDown(long version);
    }
}
