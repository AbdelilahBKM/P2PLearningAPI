using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Models;
using DotNetEnv;

// b- Load environment variables from .env file
DotNetEnv.Env.Load();

// Retrieve the connection string components from the environment variables
var serverName = Environment.GetEnvironmentVariable("SQL_SERVER_NAME");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var connectionString = $"Server={serverName};Database={dbName};Trusted_Connection=True;TrustServerCertificate=True;";
Console.WriteLine($"Server Name: {serverName}, Database Name: {dbName}, Connection String: {connectionString}");


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Define and register the DbContext directly with the connection string
builder.Services.AddDbContext<P2PLearningDbContext>(options =>
   options.UseSqlServer(connectionString));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
