using System.Text.Json.Serialization;

namespace webApi.model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Player
{
    [BsonId]  // Esto marca este campo como el identificador único
    [BsonRepresentation(BsonType.ObjectId)]  // Convierte el Id en una cadena (para facilitar la serialización en formato JSON)
    public string Id { get; set; } = string.Empty;

    [BsonElement("Name")]
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("MaxScore")]
    [JsonPropertyName("MaxScore")]
    public int? MaxScore { get; set; }
}