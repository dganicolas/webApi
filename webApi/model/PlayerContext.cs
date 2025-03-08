using Microsoft.EntityFrameworkCore;
namespace webApi.model;

public class PlayerContext : DbContext
{
    public PlayerContext(DbContextOptions<PlayerContext> options)
        : base(options)
    {
    }
}