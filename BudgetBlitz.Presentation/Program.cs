using BudgetBlitz.Application.IServices;
using BudgetBlitz.Application.Services;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;
using BudgetBlitz.Infrastructure.Services;
using BudgetBlitz.Presentation.Extensions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//builder.Services.AddSwaggerDocumentation();

builder.Services.AddStackExchangeRedisCache(redisOpitons =>
{
    string connectionString = builder.Configuration.GetConnectionString("Redis")!;

    redisOpitons.Configuration = connectionString;
});

// IMemoryCache
//builder.Services.AddMemoryCache();

// IDistributedCache
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFirebaseService, FirebaseService>();
builder.Services.AddScoped<IDataExportService, DataExportService>();

// Cache Confiugration Start
// First Way
//builder.Services.AddScoped(typeof(BaseRepository<>));
//builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(CacheBaseRepository<>));

// Second Way
//builder.Services.AddScoped(typeof(IBaseRepository<>), provider =>
//{
//    var baseRepository = provider.GetService(typeof(BaseRepository<>));

//    return new CacheBaseRepository<>(
//        baseRepository,
//        provider.GetService<IMemoryCache>()
//        );
//});

// Third way Scrutor
//builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
//builder.Services.Decorate(typeof(IBaseRepository<>), typeof(CacheBaseRepository<>));

// Cache Confiugration End


builder.Services.AddTransient<IMailService, MailService>();

//builder.Services.AddIdentityApiEndpoints<User>()
//    .AddEntityFrameworkStores<AppDbContext>();

// IdentityFrameWork Start
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
// IdentityFrameWork End


// Jwt Bearer Start
builder.Services.AddJwtAuthentication(builder.Configuration);
// Jwt Bearer End


// Firebase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile
    (
        Path.Combine
        (
            AppDomain.CurrentDomain.BaseDirectory,
            "budgetblitzmoneymanagement-firebase-adminsdk-cls9r-4c6850ab22.json"
        )
    ),
});

builder.Services.Configure<ScalarOptions>(options =>
{
    options.OpenApiRoutePattern = "/swagger/v1/swagger.json";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/swagger/v1/swagger.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapIdentityApi<User>();

app.MapControllers();

app.Run();
