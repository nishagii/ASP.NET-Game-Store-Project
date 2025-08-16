namespace GameStore.API.DTOs;

public record class UpdateGameDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
