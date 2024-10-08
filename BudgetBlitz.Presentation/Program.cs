using BudgetBlitz.Application.IServices;
using BudgetBlitz.Application.Options;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;
using BudgetBlitz.Infrastructure.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "BudgetBlitz API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            Array.Empty<string>()
        }
    });
});

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
var jwtOptionsSection = builder.Configuration.GetSection("JwtOptions");

builder.Services.Configure<JwtOptions>(jwtOptionsSection);

var jwtOptions = jwtOptionsSection.Get<JwtOptions>();

var signingKey = Encoding.ASCII.GetBytes(jwtOptions!.SigningKey);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(signingKey),
    ValidateIssuer = true,
    ValidIssuer = jwtOptions.ValidIssuer,
    ValidateAudience = true,
    ValidAudience = jwtOptions.ValidAudience,
    RequireExpirationTime = true,
    ValidateLifetime = true,
};

builder.Services.AddSingleton(tokenValidationParameters);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParameters;

});
// Jwt Bearer End


// Firebase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile
    (
        Path.Combine
        (
            AppDomain.CurrentDomain.BaseDirectory,
            "budgetblitz-96d43-firebase-adminsdk-6j2me-fad541b3f0.json"
        )
    ),
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapIdentityApi<User>();

app.MapControllers();

app.Run();
