using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class CreateGameDto(
    [Required]
    string Name,
    [Required]
    int GenreId,
    [Required]
    decimal Price,
    [Required]
    DateOnly ReleaseDate);