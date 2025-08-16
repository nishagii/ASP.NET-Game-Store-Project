using GameStore.API.DTOs;

var builder = WebApplication.CreateBuilder(args);

const string GetGameEndpointName = "GetGame";

// ConfigurationBinder request pipeline
var app = builder.Build();

List<GameDto> games = [
    new(
        1,
        "The Legend of Zelda: Breath of the Wild",
        "Action-adventure",
        59.99M,
        new DateOnly(2017, 3, 3)),
    new(
        2,
        "Super Mario Odyssey",
        "Platformer",
        49.99M,
        new DateOnly(2017, 10, 27)),
    new(
        3,
        "Minecraft",
        "Sandbox",
        29.99M,
        new DateOnly(2011, 11, 18)),
    new(
        4,
        "The Witcher 3: Wild Hunt",
        "Role-playing",
        39.99M,
        new DateOnly(2015, 5, 19)),
    new(
        5,
        "Dark Souls III",
        "Action RPG",
        39.99M,
        new DateOnly(2016, 3, 24))
];

// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndpointName);

// POST /game
app.MapPost("games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// PUT /games/1
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

app.Run();
