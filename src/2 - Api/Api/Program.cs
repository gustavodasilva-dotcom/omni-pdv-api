using Carter;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using OmniePDV.Api.Configurations;
using OmniePDV.Api.Middlewares;
using OmniePDV.Infra.Data.Seeds;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OmniePDV API V1",
        Version = "v1"
    });
});
builder.Services.AddLogging();
builder.Services.AddAuthorization();

builder.Services.AddServices();
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(OmniePDV.Core.AssemblyReference.Assembly));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddCarter();

SeedRunner.RunAsync(builder.Configuration).Wait();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "OmniePDV API V1");
        opt.RoutePrefix = "swagger/ui";
    });
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "Content", "assets")),
    RequestPath = "/resources"
});

app.UseAuthorization();

app.MapCarter();
app.Run();
