using GameStore.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// ConfigurationBinder request pipeline
var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
