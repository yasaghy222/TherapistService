using TherapistService.Mappings;
using FluentValidation;
using System.Reflection;
using TherapistService.Data;
using Microsoft.EntityFrameworkCore;


// Add services to the container.
WebApplication.CreateBuilder(args).Services.RegisterMapsterConfiguration();
WebApplication.CreateBuilder(args).Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

string? defName = WebApplication.CreateBuilder(args).Configuration["Db:Name"];
string? defHost = WebApplication.CreateBuilder(args).Configuration["Db:Host"];
string? defPass = WebApplication.CreateBuilder(args).Configuration["Db:Pass"];
string? dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? defHost;
string? dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? defName;
string? dbPass = Environment.GetEnvironmentVariable("DB_SA_PASSWORD") ?? defPass;

string? connectionString = $"Server={dbHost}; Persist Security Info=False; TrustServerCertificate=true; User ID=sa;Password={dbPass};Initial Catalog={dbName};";
WebApplication.CreateBuilder(args).Services.AddDbContext<TherapistServiceContext>(options => options.UseSqlServer(connectionString));

WebApplication.CreateBuilder(args).Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
WebApplication.CreateBuilder(args).Services.AddEndpointsApiExplorer();
WebApplication.CreateBuilder(args).Services.AddSwaggerGen();

var app = WebApplication.CreateBuilder(args).Build();

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
