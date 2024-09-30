using SubmissionsProcessor.API.Models.MongoDB;

namespace SubmissionsProcessor.API.Services.MongoDB
{
    public interface ISubmissionPropertiesService
    {
        Task CreateAsync(SubmissionProperty newSubmissionProperties);
        Task<List<SubmissionProperty>> GetAsync();
        Task<SubmissionProperty?> GetAsync(string id);
        Task<SubmissionProperty> GetBySubmissionId(string submissionId);
        Task UpdateAsync(string id, SubmissionProperty updatedSubmissionProperties);
    }
}