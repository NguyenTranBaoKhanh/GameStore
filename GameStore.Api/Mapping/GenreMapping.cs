using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api;

public static class GenreMapping
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto(genre.Id, genre.Name);
    }

    public static GenreDetailsDto ToGenreDetailDto(this Genre genre)
    {
        return new(
            genre.Id,
            genre.Name
        );
    }
}
