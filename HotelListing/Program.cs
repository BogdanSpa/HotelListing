using Serilog;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using HotelListing.Configurations;
using HotelListing.IRepository;
using HotelListing.Repository;
using Microsoft.AspNetCore.Identity;
using HotelListing.Services;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimiting();
builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureHttpCacheHeader();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddCors(o => {
    o.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(typeof(MapperInitializer));

var connectionString = builder.Configuration.GetConnectionString("sqlconnection");

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

builder.Services.AddDbContext<DatabaseContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services.AddControllers(config =>
        {
            config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
            {
                Duration = 120
            });
        }).AddNewtonsoftJson(op =>
        op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
           .AddNewtonsoftJson(op => op.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureVersioning();


try
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseIpRateLimiting();
    //global handle error => we dont need try catch in controller
    app.ConfigureExceptionHandler();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    
    app.UseResponseCaching();
    app.UseHttpCacheHeaders();
    

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application faile to start.");
}
finally
{
    Log.CloseAndFlush();
}