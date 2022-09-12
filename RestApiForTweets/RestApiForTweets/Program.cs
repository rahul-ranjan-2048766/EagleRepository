using RestApiForTweets.Services;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Steeltoe.Management.Endpoint;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Prometheus;

var builder = WebApplication.CreateBuilder(args).AddHealthActuator().AddInfoActuator().AddLoggersActuator();

ConfigureLogs();
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    { 
        Name = "Authorization",
        Description = "Bearer[space][token]",
        BearerFormat = "JWT",
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        { 
            new OpenApiSecurityScheme
            { 
                Reference = new OpenApiReference
                { 
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, 
            
            new List<string>()
        }
    });
});

builder.Services.AddTransient<ITweetService, TweetService>();
builder.Services.AddTransient<ICommentService, CommentService>();

builder.Services.AddCors(x => x.AddDefaultPolicy(sam => sam.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddAuthentication(x =>
{
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters 
    { 
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["SecretKey"]))
    };
});

builder.Services.AddOpenTelemetryTracing(x =>
{
    x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("RestApiForTweets"));
    x.AddSource("RestApiForTweetsTraceSource");
    x.AddAspNetCoreInstrumentation(sam => sam.Filter = httpContext => !httpContext.Request.Path.Value?.Contains("/_framework/aspnetcore-browser-refresh.js") ?? true);
    x.AddHttpClientInstrumentation(sam => sam.Enrich = (activity, eventName, rawObject) =>
    {
        if (eventName == "OnStartActivity" && rawObject is HttpRequestMessage request && request.Method == HttpMethod.Get)
            activity.SetTag("RestApiForTweets", "This api handles the tweets and comments....");
    });
    x.AddEntityFrameworkCoreInstrumentation(sam =>
    {
        sam.SetDbStatementForText = true;
        sam.SetDbStatementForStoredProcedure = true;
    });
    x.AddJaegerExporter(sam =>
    {
        sam.AgentHost = builder.Configuration["JaegerHost"];
        sam.AgentPort = int.Parse(builder.Configuration["JaegerPort"]);
    });
    x.AddZipkinExporter(sam =>
    {
        sam.Endpoint = new Uri($"http://{ builder.Configuration["ZipkinHost"] }:{ builder.Configuration["ZipkinPort"] }/api/v2/spans");
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpMetrics();
app.MapMetrics();
app.MapControllers();
app.Run();

void ConfigureLogs()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureELS(configuration, environment))
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureELS(IConfigurationRoot configuration, string? environment)
{
    return new ElasticsearchSinkOptions(new Uri($"http://{ builder.Configuration["ELKHost"] }:{ builder.Configuration["ELKPort"] }")) 
    { 
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower()}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}