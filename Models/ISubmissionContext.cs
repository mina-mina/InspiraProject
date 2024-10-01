namespace SubmissionsProcessor.API.Models
{
    public interface ISubmissionContext
    {
        string SubmissionId { get; set; }
        string? CorrelationId { get; set; } //[MINA] - should this be on the request itself as a header?

    }
}