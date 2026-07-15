using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using LMS.API.Common.Middleware;
using LMS.Application.Interfaces;
using LMS.Application.Mappings;
using LMS.Application.Validators.CourseValidator;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Repositories;
using LMS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using LMS.API.Authorization;
Log.Logger=new LoggerConfiguration().WriteTo.Console().WriteTo.File("logs/log-.txt",rollingInterval:RollingInterval.Day).CreateLogger();//komil before createBuilder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Host.UseSerilog();//komeil
builder.Services.AddControllers();//komeil
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IUserRepository,UserRepository>();//komeil
builder.Services.AddScoped<IPasswordService,PasswordService>();//komeil
///builder.Services.AddAutoMapper(Assembly.Load("LMS.Application"));//komeil
// builder.Services.AddAutoMapper(typeof(AuthMappingProfile));
// builder.Services.AddAutoMapper(typeof(CourseMappingPofile));
builder.Services.AddAutoMapper(typeof(AuthMappingProfile).Assembly);
builder.Services.AddScoped<IAUthService,AuthService>();//komeil
builder.Services.AddScoped<ITokenService,TokenService>();//komeil
builder.Services.AddFluentValidationAutoValidation();//komeil
builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseDtoValidator>();
builder.Services.AddScoped<ICourseRepository,CourseRepoitory>();
builder.Services.AddScoped<ICourseService,CourseService>();
builder.Services.AddScoped<IEnrollmentRepository,EnrollmentRepository>();
builder.Services.AddScoped<IEnrollService,EnrollmentService>();
builder.Services.AddScoped<ILessonRepository,LessonRepository>();
builder.Services.AddScoped<ILessonService,LessonService>();
builder.Services.AddEndpointsApiExplorer();//komeil
 builder.Services.AddSwaggerGen(
    options =>
{
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
        Name="Authorization",
        Type=SecuritySchemeType.Http,
        Scheme="bearer",
        BearerFormat="JWT",
        In=ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },new List<string>()
        }
    });
    
}
);//komeil

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
builder.Services.AddAuthorization(options =>
{
    // options.AddPolicy("ManageCourse", policy =>
    // {
    //     policy.RequireRole("Admin","Teacher");
    // });
    options.AddPolicy("CourseOwner", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new CourseOwnerRequirement());
    });
});
//----------------------------------
builder.Services.AddScoped<IAuthorizationHandler,CourseOwnerHandler>();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();//komeil
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}



// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

 app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
