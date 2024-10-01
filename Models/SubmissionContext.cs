namespace SubmissionsProcessor.API.Models
{
    public class SubmissionContext : ISubmissionContext
    {
        //[MINA]   protected IHttpContextAccessor HttpContextAccessor { get; }

        /*[MINA] */
        /*  public SubmissionContext(IHttpContextAccessor httpContextAccessor) {
             HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
         } */
        public SubmissionContext()
        {

        }
        public string SubmissionId { get; set; }
        public string CorrelationId { get; set; }

        /* public string GetSubmissionId()
        {
            return HttpContextAccessor.HttpContext?.SubmissionId;
        } */
    }
}
