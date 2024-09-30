using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SubmissionsProcessor.API.Models.MongoDB;

namespace SubmissionsProcessor.API.Services.MongoDB
{
    public class SubmissionsService : ISubmissionsService
    {

        private readonly IMongoCollection<Submission> _SubmissionsCollection;


        public SubmissionsService(IMongoDatabase mongoDatabase)
        {
            _SubmissionsCollection = mongoDatabase.GetCollection<Submission>(nameof(Submission));
        }

        public async Task<List<Submission>> GetAsync() =>
            await _SubmissionsCollection.Find(_ => true).ToListAsync();

        public async Task<Submission?> GetAsync(string id) =>
            await _SubmissionsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Submission newSubmission) =>
            await _SubmissionsCollection.InsertOneAsync(newSubmission);

        public async Task UpdateAsync(string id, Submission updatedSubmission) =>
            await _SubmissionsCollection.ReplaceOneAsync(x => x.Id == id, updatedSubmission);

    }
}
