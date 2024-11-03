using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Define and register the DbContext directly with the connection string
builder.Services.AddDbContext<P2PLearningDbContext>(options =>
   options.UseSqlServer("Server=DESKTOP-RR3AB05\\SQLEXPRESS;Database=P2PLearningDB;Trusted_Connection=True;TrustServerCertificate=True;"));


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
