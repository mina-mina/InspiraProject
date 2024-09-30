using MongoDB.Driver;
using SubmissionsProcessor.API.Models.MongoDB;

namespace SubmissionsProcessor.API.Services.MongoDB
{
    public class FormsService : IFormsService
    {

        private readonly IMongoCollection<Form> _FormsCollection;


        public FormsService(IMongoDatabase mongoDatabase)
        {
            _FormsCollection = mongoDatabase.GetCollection<Form>(nameof(Form));
        }

        public async Task<List<Form>> GetAsync() =>
            await _FormsCollection.Find(_ => true).ToListAsync();

        public async Task<Form?> GetAsync(string id) =>
            await _FormsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Form newForm) =>
            await _FormsCollection.InsertOneAsync(newForm);

        public async Task UpdateAsync(string id, Form updatedForm) =>
            await _FormsCollection.ReplaceOneAsync(x => x.Id == id, updatedForm);

    }
}
