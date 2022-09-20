using api.Data;
using api.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddDbContext<DataContext>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("REDIS_CONNECTION_STRING");
    options.InstanceName = builder.Configuration.GetValue<string>("ENVIRONMENT_NAME");
});

builder.Services.AddMemoryCache();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
}).AddCookie("Cookies", options =>
{
    options.Cookie.Name = "team_cookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Events = new CookieAuthenticationEvents()
    {
        OnRedirectToLogin = recontext =>
        {
            recontext.HttpContext.Response.StatusCode = 401;
            return Task.CompletedTask;
        },
    };
});
builder.Services.AddCookiePolicy(options => options.Secure = CookieSecurePolicy.Always);

var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("REDIS_CONNECTION_STRING"));
builder.Services.AddDataProtection().PersistKeysToStackExchangeRedis(redis, "bcc-code-run-dataprotection-keys");

//if (!builder.Environment.IsDevelopment())
//{
//    builder.Services.AddDataProtection()
//        .SetApplicationName("QR-App")
//        .PersistKeysToAzureBlobStorage(builder.Configuration.GetConnectionString("Storage"), "keys", "dataprotection");
//}

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

builder.Services.AddSingleton<CacheService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

var db = app.Services.CreateScope().ServiceProvider.GetService<DataContext>()!.Database;
await db.MigrateAsync();

app.UseCors(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    //policy.AllowAnyOrigin();
    policy.SetIsOriginAllowed(s => true);
    policy.AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
