using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;

namespace SubmissionsProcessor.API.Models.MongoDB
{
    public class Submission
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [FromHeader]
        public string CorrelationId { get; set; }
        public Form Form { get; set; }
        public List<SubmissionProperty> SubmissionProperties { get; set; }
    }
}
