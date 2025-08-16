var builder = WebApplication.CreateBuilder(args);

// ConfigurationBinder request pipeline
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
