using TherapistService.Mappings;
using FluentValidation;
using System.Reflection;
using TherapistService.Data;
using Microsoft.EntityFrameworkCore;


internal class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.RegisterMapsterConfiguration();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        string? defName = builder.Configuration["Db:Name"];
        string? defHost = builder.Configuration["Db:Host"];
        string? defPass = builder.Configuration["Db:Pass"];
        string? dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? defHost;
        string? dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? defName;
        string? dbPass = Environment.GetEnvironmentVariable("DB_SA_PASSWORD") ?? defPass;

        string? connectionString = $"Server={dbHost}; Persist Security Info=False; TrustServerCertificate=true; User ID=sa;Password={dbPass};Initial Catalog={dbName};";
        builder.Services.AddDbContext<TherapistServiceContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}