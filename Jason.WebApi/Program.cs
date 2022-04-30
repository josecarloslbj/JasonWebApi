using Jason.WebApi;
using Jason.WebApi.Infra.Interfaces;
using Jason.WebApi.Infra.Repositories;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using FluentMigrator.Runner;
using System.Reflection;
using Jason.WebApi.Migrations.Interfaces;
using Jason.WebApi.Migrations.Services;
using Dapper;
using System.Data.SqlClient;
using Elastic.Apm;
using Elastic.Apm.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddControllers()
  .AddNewtonsoftJson(options =>
  {
      options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
      options.SerializerSettings.Formatting = Formatting.Indented;
  });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString(appSettings.UseDatabase);
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IMigratorService, MigratorService>();


builder.Services.AddScoped<IUserRepository, UserRepository>(_ =>
{
    return new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//string migrationAssemblyPath = Path.Combine(appSettings.ExecutingAssembly.Location.LeftOfRightmostOf("\\"), appSettings.MigrationAssembly);
//Assembly migrationAssembly = Assembly.LoadFrom(migrationAssemblyPath);

 builder.Services.AddFluentMigratorCore()
                        .ConfigureRunner(rb => rb
                            .AddSqlServer()
                            .WithGlobalConnectionString(connection)
                            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                            .AddLogging(lb => lb.AddFluentMigratorConsole()
                            );


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var conDefault = builder.Configuration.GetConnectionString("DefaultConnection");
    var conMaster = builder.Configuration.GetConnectionString("MasterConnection");
        
    var cs = conDefault.ToString();
    var dbName = cs.RightOf("Database=").LeftOf(";");
    var master = conMaster;

    var parameters = new DynamicParameters();
    parameters.Add("name", dbName);
    var connectionSqlServer = new SqlConnection(master);
    var records = connectionSqlServer.Query("SELECT name FROM sys.databases WHERE name = @name", parameters);

    if (!records.Any())
    {
        connectionSqlServer.Execute($"CREATE DATABASE [{dbName}]");
    }

    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    // Execute the migrations
    runner.MigrateUp();

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseElasticApm();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();



