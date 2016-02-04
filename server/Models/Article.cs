using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoMvc.Models
{
    [BsonIgnoreExtraElements]
    public class Article
    {
        public BsonObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("subtitle")]
        public string Subtitle { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }
        
        [BsonElement("pub_date")]
        public BsonDateTime Pub_date { get; set; }
    }
}