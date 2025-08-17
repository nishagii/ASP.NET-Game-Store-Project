using System;
using GameStore.API.Data;
using GameStore.API.DTOs;
using GameStore.API.Entities;
using GameStore.API.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints;



public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    //Extension methods must live in a static class because they’re just syntactic sugar for static methods.
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
        .WithParameterValidation();

        // GET /games
        group.MapGet("/", (GameStoreContext dbContext) =>
            dbContext.Games
                     .Include(game => game.Genre)
                     .Select(game => game.ToGameSummaryDto())
                     .AsNoTracking()
                     );


        // GET /games/1
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            // ? means it (game) can be null
            Game? game = dbContext.Games.Find(id);


            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndpointName);


        // POST /game
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            //dto to entuty
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(
                GetGameEndpointName,
                new { id = game.id },
                game.ToGameDetailsDto());
        });


        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                     .CurrentValues
                     .SetValues(updatedGame.ToEntity(id));

            dbContext.SaveChanges();

            return Results.NoContent();
        });


        // DELETE /games/1
        group.MapDelete("/{id}", (int id,GameStoreContext dbContext) =>
        {
            dbContext.Games.Where(game => game.id == id)
            .ExecuteDelete();

            return Results.NoContent();
        });

        return group;

    }
}
