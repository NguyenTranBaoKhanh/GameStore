using GameStore.Api.Data;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app){
        var group = app.MapGroup("genres")
                        .WithParameterValidation();
        //GET /genres
        group.MapGet("/", async (GameStoreContext dbContext) => {
            return await dbContext.Genres.Select(_ => _.ToDto())
                                    .AsNoTracking()
                                    .ToListAsync();
        });

        //GET /genres/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Genre? genre = await dbContext.Genres.FindAsync(id);

            return genre is null ? 
                Results.NotFound() : Results.Ok(genre.ToGenreDetailDto());
        })
        .WithName("GetGenre");

        
        return group;
    }
}
