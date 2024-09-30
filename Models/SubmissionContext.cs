namespace SubmissionsProcessor.API.Models
{
    public class SubmissionContext : ISubmissionContext
    {
        public SubmissionContext()
        {

        }
        public string SubmissionId { get; set; }
        public string CorrelationId { get; set; }
    }
}
