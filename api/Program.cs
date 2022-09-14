using api;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddDataProtection()
        .SetApplicationName("QR-App")
        .PersistKeysToAzureBlobStorage(builder.Configuration.GetConnectionString("Storage"), "keys", "dataprotection");
}

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
