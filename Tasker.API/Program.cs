using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tasker.API.DB;
using Tasker.API.DomainObjects;


var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("https://localhost:7001")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlite("Data Source=identity.db"));

builder.Services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Проверять issuer
        ValidateAudience = true, // Проверять audience
        ValidateLifetime = true, // Проверять срок
        ValidateIssuerSigningKey = true, // Проверять подпись
        ValidIssuer = "yourapp.com", // Твой issuer (можно из конфига)
        ValidAudience = "yourapp.com", // Твой audience
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyAtLeast32CharsLong")) // Секретный ключ, храни в secrets!
    };
});

builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); // Если API


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerUI();
app.UseSwagger();
app.MapControllers();
app.Run();
