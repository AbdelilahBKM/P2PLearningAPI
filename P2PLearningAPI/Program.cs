using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Repository;
using P2PLearningAPI.Models;
using Microsoft.AspNetCore.Identity;
using P2PLearningAPI.Extensions;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

DotNetEnv.Env.Load();

var serverName = Environment.GetEnvironmentVariable("SQL_SERVER_NAME");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var connectionString = $"Server={serverName};Database={dbName};Trusted_Connection=True;TrustServerCertificate=True;";
Console.WriteLine($"Server Name: {serverName}, Database Name: {dbName}, Connection String: {connectionString}");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<P2PLearningDbContext>()
    .AddApiEndpoints();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserIdentity, UserIdentiyRepository>();
builder.Services.AddScoped<ITokenService, TokenServices>();
builder.Services.AddScoped<IDiscussionInterface, DiscussionRepository>();
builder.Services.AddScoped<IJoiningInterface, JoiningRepository>();
builder.Services.AddScoped<IPostInterface, PostRepository>();
builder.Services.AddScoped<IRequestInterface, RequestRepository>();
builder.Services.AddScoped<IVoteInterface, VoteRepository>();

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
    };
});

// Define and register the DbContext directly with the connection string
builder.Services.AddDbContext<P2PLearningDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNodeFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Update with your Node.js frontend URL
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowNodeFrontend");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.MapIdentityApi<User>();

app.Run();
