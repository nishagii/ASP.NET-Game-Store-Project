namespace GameStore.API.Entities;

public class Game
{
    public int id { get; set; }

    public required string Name { get; set; }

    // Foreign key to the Genre entity
    // Stores the ID of the Genre associated with this Game
    public int GenreId { get; set; }

    // Navigation property to the Genre entity
    // The '?' makes it nullable, meaning a Game may not have the Genre object loaded yet
    // Used in EF Core to navigate the relationship between Game and Genre
    public Genre? Genre { get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }
}
