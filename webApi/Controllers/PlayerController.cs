using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using webApi.model;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IMongoCollection<Player> _playersCollection;
        public PlayerController(IOptions<PlayerDatabaseSettings> playerDatabaseSettings)
        {
            var settings = playerDatabaseSettings.Value;
            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            _playersCollection = mongoDatabase.GetCollection<Player>(settings.PlayersCollectionName);
        }

        // GET: api/Player
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            // Obtener los jugadores ordenados por la puntuación y limitar el resultado a los 5 primeros
            var topPlayers = await _playersCollection
                .Find(_ => true)
                .SortByDescending(p => p.MaxScore) // Ordenar por puntuación
                .Limit(5) // Limitar a los 5 jugadores con mayores puntuaciones
                .ToListAsync(); // Obtener la lista de jugadores

            return Ok(topPlayers); // Retornar la lista de jugadores en la respuesta
        }

        // PUT: api/anadirPlayer
        [HttpPut]
        public async Task<IActionResult> PutPlayer(Player player)
        {
            if (string.IsNullOrWhiteSpace(player.Name))
            {
                return BadRequest("El nombre del jugador no puede ser nulo ni vacío.");
            }

            // Si la puntuación es nula, asigna un valor de 0
            if (player.MaxScore == null)
            {
                player.MaxScore = 0;
            }

            // Insertamos el jugador directamente en la base de datos
            await _playersCollection.InsertOneAsync(player);

            // Devolvemos el jugador que fue insertado
            return Ok(player);
        }
    }
}
