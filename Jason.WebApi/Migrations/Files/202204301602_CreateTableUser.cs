using FluentMigrator;

namespace Jason.WebApi.Migrations.Files
{
    [Migration(202204301602)]
    public class CreateTableUser : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString()
                .WithColumn("Email").AsString()
                .WithColumn("PhoneNumber").AsString()
                .WithColumn("CreatedAt").AsDateTime().Nullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}
