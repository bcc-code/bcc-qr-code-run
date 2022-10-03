using api.Data;
using api.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddDbContextPool<DataContext>(options =>
{
    //var connectionString = "Server=localhost;Port=5432;Database=bcc-code-run;Username=admin;Password=password;";
    var connectionString = builder.Configuration["AZURE_POSTGRESQL_CONNECTIONSTRING"];
    if (string.IsNullOrEmpty(connectionString))
    {
        var dbPort = builder.Configuration["POSTGRES_PORT"];
        var dbName = builder.Configuration["POSTGRES_DB"];
        var dbUser = builder.Configuration["POSTGRES_USER"];
        var dbHost = builder.Configuration["POSTGRES_HOST"];
        var dbPassword = builder.Configuration["POSTGRES_PASSWORD"];
        connectionString =
            $"Host={dbHost}{(dbPort != "5432" ? ";Port=" + (dbPort ?? "") : "")};Database={dbName};Username={dbUser};Password={dbPassword};Timeout=300;CommandTimeout=300";
    }

    options.UseNpgsql(connectionString);
});

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
    options.Cookie.SameSite = SameSiteMode.Strict;
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

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

builder.Services.AddSingleton<CacheService>();
builder.Services.AddScoped<ResultsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
//    app.UseHttpsRedirection();
}

var supportedCultures = new[]
{
 new CultureInfo("nb-NO")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("nb-NO"),
    // Formatting numbers, dates, etc.
    SupportedCultures = supportedCultures,
    // UI strings that we have localized.
    SupportedUICultures = supportedCultures
});


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

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();
}

app.Run();
