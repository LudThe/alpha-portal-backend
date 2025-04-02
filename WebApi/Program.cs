using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AlphaDB")));


builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientInformationRepository, ClientInformationRepository>();
builder.Services.AddScoped<IClientAddressRepository, ClientAddressRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberRoleRepository, MemberRoleRepository>();
builder.Services.AddScoped<IMemberInformationRepository, MemberInformationRepository>();
builder.Services.AddScoped<IMemberAddressRepository, MemberAddressRepository>();
builder.Services.AddScoped<IProjectStatusRepository, ProjectStatusRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IMemberRoleService, MemberRoleService>();
builder.Services.AddScoped<IProjectStatusService, ProjectStatusService>();
builder.Services.AddScoped<IProjectService, ProjectService>();


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
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseCors("AllowAll");
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Alpha API");
    x.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
