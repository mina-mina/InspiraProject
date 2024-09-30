namespace SubmissionsProcessor.API.Services
{
    public interface ISSNInternalCheckMockService
    {
        int RandomContactId { get; }

        Task<string> SSNInternalCheckAsync(string ssn, string role);
    }
}