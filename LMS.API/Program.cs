using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using LMS.Application.Interfaces;
using LMS.Application.Mappings;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Repositories;
using LMS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
Log.Logger=new LoggerConfiguration().WriteTo.Console().WriteTo.File("logs/lms-.txt",rollingInterval:RollingInterval.Day).CreateLogger();//komil before createBuilder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Host.UseSerilog();//komeil
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IUserRepository,UserRepository>();//komeil
builder.Services.AddScoped<IPasswordService,PasswordService>();//komeil
///builder.Services.AddAutoMapper(Assembly.Load("LMS.Application"));//komeil
builder.Services.AddAutoMapper(typeof(AuthMappingProfile));
builder.Services.AddAutoMapper(typeof(CoursePofile));
builder.Services.AddScoped<IAUthService,AuthService>();//komeil
builder.Services.AddScoped<ITokenService,TokenService>();//komeil
builder.Services.AddControllers();//komeil
builder.Services.AddScoped<ICourseRepository,CourseRepoitory>();
builder.Services.AddScoped<ICourseService,CourseService>();
builder.Services.AddEndpointsApiExplorer();//komeil
builder.Services.AddSwaggerGen();//komeil

var jwt=builder.Configuration.GetSection("JWT");//komeil
//Komeil -------------------------Auth------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters=new TokenValidationParameters
    {
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,
        ValidIssuer=jwt["Issuer"],
        ValidAudience=jwt["Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
    };
});
builder.Services.AddAuthorization();
//----------------------------------
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
