using System;
using GameStore.API.Data;
using GameStore.API.DTOs;
using GameStore.API.Entities;

namespace GameStore.API.Endpoints;



public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
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

    //Extension methods must live in a static class because they’re just syntactic sugar for static methods.
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
        .WithParameterValidation();

        // GET /games
        group.MapGet("/", () => games);


        // GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            // ? means it (game) can be null
            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);


        // POST /game
        group.MapPost("/", (CreateGameDto newGame,GameStoreContext dbContext) =>
        {

            Game game = new()
            {
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreId),
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.id }, game);
        });


        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });


        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;

    }
}
