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
using P2PLearningAPI.Repositories;
using P2PLearningAPI.Services;
using Microsoft.OpenApi.Models;

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
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IDiscussionInterface, DiscussionRepository>();
builder.Services.AddScoped<IJoiningInterface, JoiningRepository>();
builder.Services.AddScoped<IPostInterface, PostRepository>();
builder.Services.AddScoped<IRequestInterface, RequestRepository>();
builder.Services.AddScoped<IVoteInterface, VoteRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IUploadInterface, UploadRepository>();
builder.Services.AddScoped<ISimularityTestInterface, SimularityTest>();
builder.Services.AddHttpClient<SimilarityService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8000"); // FastAPI base URL
});

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

// json soft reference
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Define and register the DbContext directly with the connection string
builder.Services.AddDbContext<P2PLearningDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Frontend origin
              .AllowAnyHeader()                    // Allow all headers
              .AllowAnyMethod()                    // Allow all HTTP methods
              .AllowCredentials();                 // Allow cookies or authentication headers
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Support file uploads
    c.OperationFilter<SwaggerFileUploadOperationFilter>();
});

var app = builder.Build();
// Use CORS before other middleware
app.UseCors("AllowLocalhost3000");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

// Enable serving static files from the wwwroot folder
app.UseStaticFiles();

// Use CORS
app.UseCors("AllowNodeFrontend");



app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.MapIdentityApi<User>();

app.Run();
