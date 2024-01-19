using Microsoft.Extensions.FileProviders;
using OmniePDV.API.Data;
using OmniePDV.API.Data.Seeders;
using OmniePDV.API.Middlewares;
using OmniePDV.API.Options;
using OmniePDV.API.Services;
using OmniePDV.API.Services.Interfaces;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .Configure<MongoDBOptions>(
        builder.Configuration.GetSection(MongoDBOptions.Position))
    .Configure<DefaultClientOptions>(
        builder.Configuration.GetSection(DefaultClientOptions.Position))
    .Configure<RabbitMQOptions>(
        builder.Configuration.GetSection(RabbitMQOptions.Position));

builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddSingleton<IMessageProducer, MessageProducer>();
builder.Services.AddSingleton<IPointOfSalesService, PointOfSalesService>();

builder.Services.AddLogging();

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

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

SeedRunner.ExecuteAsync(builder.Configuration).Wait();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

app.MapControllers();

app.Run();
