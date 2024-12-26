using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GameEndPoints
{
    // private static readonly List<GameSummaryDto> games = [
    //     new (1, "Street Fighter II", "Fighting", 19.99M, new DateOnly(1992,7,15)),
    //     new (2, "Final Fantasy XIV", "Roleplaying", 59.99M, new DateOnly(2010,9,30)),
    //     new (3, "FIFA 23", "Sports", 69.99M, new DateOnly(2022,9,27)),
    // ];

    public static WebApplication MapGamesEndPoints(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");

        //GET /games
        app.MapGet("games",async (GameStoreContext dbContext) => {
            return await dbContext.Games
                    .Include(_ => _.Genre)
                    .Select(_ => _.ToGameSummaryDto())
                    .AsNoTracking()
                    .ToListAsync();
        });

        //GET /games/1
        app.MapGet("games/{id}", async (int id, GameStoreContext dbContext) => {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

        }).WithName("GetGame");

        //POST /games
        app.MapPost("games", async (CreateGameDto newGame, GameStoreContext dbContext) => {
            
            Game game = newGame.ToEntity();
            // game.Genre = dbContext.Genres.Find(newGame.GenreId);

            // Game game = new(){
            //     Name = newGame.Name,
            //     Genre = dbContext.Genres.Find(newGame.GenreId),
            //     GenreId = newGame.GenreId,
            //     Price = newGame.Price,
            //     ReleaseDate = newGame.ReleaseDate,
            // };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            // GameDto gameDto = new(
            //     game.Id,
            //     game.Name,
            //     game.Genre!.Name,
            //     game.Price,
            //     game.ReleaseDate
            // );

            return Results.CreatedAtRoute("GetGame", new {id = game.Id}, game.ToGameDetailsDto());
        }).WithParameterValidation();

        //PUT /games/1
        app.MapPut("games/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => {
            // var index = games.FindIndex(games => games.Id == id);
            // if(index == -1) return Results.NotFound();


            var existingGame = await dbContext.Games.FindAsync(id);
            if(existingGame is null) return Results.NotFound();

            // games[index] = new GameSummaryDto(
            //     id,
            //     updatedGame.Name,
            //     updatedGame.Genre,
            //     updatedGame.Price,
            //     updatedGame.ReleaseDate
            // );

            dbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(updatedGame.ToEntity(id));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE /games/1
        app.MapDelete("games/{id}", async (int id, GameStoreContext dbContext) => {
            await dbContext.Games.Where(_ => _.Id == id)
                                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return app;
    }
}
