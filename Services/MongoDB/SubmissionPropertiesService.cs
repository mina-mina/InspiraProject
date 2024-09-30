using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SubmissionsProcessor.API.Models.MongoDB;

namespace SubmissionsProcessor.API.Services.MongoDB
{
    public class SubmissionPropertiesService : ISubmissionPropertiesService
    {

        private readonly IMongoCollection<SubmissionProperty> _SubmissionPropertiesCollection;


        public SubmissionPropertiesService(IMongoDatabase mongoDatabase)
        {
            _SubmissionPropertiesCollection = mongoDatabase.GetCollection<SubmissionProperty>(nameof(SubmissionProperty));
        }
        public async Task<SubmissionProperty> GetBySubmissionId(string submissionId) =>
            await _SubmissionPropertiesCollection.AsQueryable().Where(r => r.SubmissionId == submissionId).FirstOrDefaultAsync();
        public async Task<List<SubmissionProperty>> GetAsync() =>
            await _SubmissionPropertiesCollection.Find(_ => true).ToListAsync();

        public async Task<SubmissionProperty?> GetAsync(string id) =>
            await _SubmissionPropertiesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(SubmissionProperty newSubmissionProperties) =>
            await _SubmissionPropertiesCollection.InsertOneAsync(newSubmissionProperties);

        public async Task UpdateAsync(string id, SubmissionProperty updatedSubmissionProperties) =>
            await _SubmissionPropertiesCollection.ReplaceOneAsync(x => x.Id == id, updatedSubmissionProperties);

    }
}
