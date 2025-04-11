using Business.Handlers;
using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WebApi.Extensions.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AlphaDB")));
builder.Services.AddIdentity<AppUserEntity, IdentityRole>(x => { }).AddEntityFrameworkStores<DataContext>();

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IAppUserAddressRepository, AppUserAddressRepository>();
builder.Services.AddScoped<IAppUserProfileRepository, AppUserProfileRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientInformationRepository, ClientInformationRepository>();
builder.Services.AddScoped<IClientAddressRepository, ClientAddressRepository>();
builder.Services.AddScoped<IProjectStatusRepository, ProjectStatusRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IAppUserRoleService, AppUserRoleService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProjectStatusService, ProjectStatusService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<IFileHandler>(_ => new AzureFileHandler(builder.Configuration.GetConnectionString("AzureBlobStorage")!, "media"));

builder.Services.AddTransient<JwtTokenHandler>();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!);
        var issuer = builder.Configuration["JWT:Issuer"]!;
        var audience = builder.Configuration["JWT:Audience"]!;

        x.RequireHttpsMetadata = false; // change to true in prod
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.FromMinutes(5),
            ValidIssuer = issuer,
            ValidateIssuer = true,
            ValidAudience = audience,
            ValidateAudience = false,
        };
    });


builder.Services.AddCors(x =>
{
    x.AddPolicy("AllowAll", x =>
    {
        x.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.ExampleFilters();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v. 1.0",
        Title = "Alpha API Documentation",
        Description = "This is the standard documentation for Alpha Portal.",
    });


    var apiKeyScheme = new OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        Description = "Api-Key Required",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        Reference = new OpenApiReference
        {
            Id = "ApiKey",
            Type = ReferenceType.SecurityScheme,
        }
    };
    options.AddSecurityDefinition("ApiKey", apiKeyScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { apiKeyScheme, new List<string>() }
    });


    var apiAdminScheme = new OpenApiSecurityScheme
    {
        Name = "X-ADM-API-KEY",
        Description = "Admin Api-Key Required",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        Reference = new OpenApiReference
        {
            Id = "AdminApiKey",
            Type = ReferenceType.SecurityScheme,
        }
    };
    options.AddSecurityDefinition("AdminApiKey", apiAdminScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { apiAdminScheme, new List<string>() }
    });


    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });


    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {{  new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },new string[] { }
            }
    });
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();


var app = builder.Build();

//await SeedData.SetRolesAsync(app);

app.UseCors("AllowAll");
app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Alpha API");
        x.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<DefaultApiKeyMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
