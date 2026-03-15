using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Overclocked.API.Data;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/overclocked.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = key
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (db.Database.IsRelational())
    {
        db.Database.Migrate();
    }
    else
    {
        db.Database.EnsureCreated();
    }

    PopulateProducts.Seed(db);
}

var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

if (app.Environment.IsDevelopment() && !isDocker)
{
    var frontendPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../Overclocked.Frontend"));
    var startInfo = new System.Diagnostics.ProcessStartInfo
    {
        FileName = "cmd.exe",
        Arguments = $"/c cd \"{frontendPath}\" && npm install && npm run dev",
        UseShellExecute = true,
        WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized
    };
    System.Diagnostics.Process.Start(startInfo);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }