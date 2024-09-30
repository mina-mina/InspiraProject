using SubmissionsProcessor.API.Models.MongoDB;

namespace SubmissionsProcessor.API.Services.MongoDB
{
    public interface ISubmissionsService
    {
        Task CreateAsync(Submission newSubmission);
        Task<List<Submission>> GetAsync();
        Task<Submission?> GetAsync(string id);
        Task UpdateAsync(string id, Submission updatedSubmission);
    }
}