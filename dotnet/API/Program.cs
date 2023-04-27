using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using SharedResources.Helpers;
using SharedResources.Settings;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wordle;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc(
        "v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Wordle Game API - V1",
            Version = "v1",
            Description = "Game API for wordle. Project for code demonstration."
        });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    x.EnableAnnotations();
});

builder.Services.Configure<AppSettings>(builder.Configuration);
var appSettings = new AppSettings();
ConfigurationBinder.Bind(builder.Configuration, appSettings);
builder.Services.AddSingleton(appSettings);

WordleStartup.Startup(builder.Services, appSettings);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(x =>
{
    x.Run(async context =>
    {
        var pathFeatures = context.Features.Get<IExceptionHandlerPathFeature>();
    
        if(pathFeatures == null) { return; }

        using var scope = x.ApplicationServices.CreateScope();

        var exception = pathFeatures.Error;
        var details = ErrorHandler.GetProblemDetailsAsync(exception, pathFeatures.Path, scope);

        context.Response.StatusCode = details.Status ?? 500;
        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(details);
        await context.Response.WriteAsync(json);
    });
});

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
