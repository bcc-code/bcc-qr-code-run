using LoadTester;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<LoadTesterService>();

var app = builder.Build();

app.MapGet("/", () =>  "Hello world");

app.Run();
