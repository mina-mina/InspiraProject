namespace SubmissionsProcessor.API.Models
{
    public interface ISubmissionContext
    {
        string SubmissionId { get; set; }
        string? CorrelationId { get; set; }

    }
}