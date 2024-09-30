using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SubmissionsProcessor.API.Models.MongoDB
{
    public class Form
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code{ get; set; }
    }
}