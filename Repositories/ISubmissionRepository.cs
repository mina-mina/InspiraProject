using SubmissionsProcessor.API.Models;

namespace SubmissionsProcessor.API.Repositories
{
    public interface ISubmissionRepository
    {
        Task<SubmissionResponse> GetSubmissionResponse(SubmissionRequest model, string submissionId);
    }
}