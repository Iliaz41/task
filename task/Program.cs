using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json.Serialization;
using task.Data;
using task.Mapper;
using task.Middlewares;
using task.Repositories;
using task.Repositories.IRepositories;
using task.Services;
using Microsoft.IdentityModel.Tokens;
using task.Services.IServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Configure CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSetOrigins", options =>
    {
        options.WithOrigins("http://localhost:4200").AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials();
    });
});

#endregion

#region Configure Database

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

#endregion

#region Configure AutoMapper

builder.Services.AddAutoMapper(typeof(MappingProfile));

#endregion

#region

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IRoleService, RoleService>();

#endregion

#region Configure Serilog

builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day);

    if (context.HostingEnvironment.IsProduction() == false)
    {
        config.WriteTo.Console();
    }
});

#endregion

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "UserRole API",
        Description = "An ASP.NET Core Web API for add users with roles"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value!))
    };
});


var app = builder.Build();

app.UseCustomException();

app.UseCors("AllowSetOrigins");


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
