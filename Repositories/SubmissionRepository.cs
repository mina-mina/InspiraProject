using SubmissionsProcessor.API.Models;
using SubmissionsProcessor.API.Services;
using SubmissionsProcessor.API.Services.MongoDB;

namespace SubmissionsProcessor.API.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private const string PROP_OWNER_TAX_ID = "owner: tax id";
        private const string PROP_OWNER_CONTACT_ID = "owner: contact id";
        private readonly ILogger<SubmissionRepository> _logger;
        private readonly ISubmissionPropertiesService _service;
        private readonly ISSNInternalCheckMockService _ssnCheckMockService;

        public SubmissionRepository(ISubmissionPropertiesService submissionPropertyService, ISSNInternalCheckMockService ssnCheckMockService, ILogger<SubmissionRepository> logger)
        {
            _service = submissionPropertyService;
            _ssnCheckMockService = ssnCheckMockService;
            _logger = logger;

        }

        public async Task<SubmissionResponse> GetSubmissionResponse(SubmissionRequest model, string submissionId)
        {
            bool result = false;
            var contactId = 0;

            //TODO: Validate SubmissionId AND userId by querying submissions catalogue 

            var submissionProperty = await _service.GetBySubmissionId(submissionId);

            var taxId = await GetValidTaxId(model.SSN, submissionId, submissionProperty.Properties.FirstOrDefault().GetValueOrDefault(PROP_OWNER_TAX_ID));
            var role = model.Role == "Owner" ? 1 : 0;

            //soap api call
            var ssnInternalCheckResult = await _ssnCheckMockService.SSNInternalCheckAsync(taxId, role.ToString());
            
            //update contactId in db if valid AND role=owner
            if (role == 1 && int.TryParse(ssnInternalCheckResult, out contactId))
            {
                submissionProperty.Properties.FirstOrDefault(x => x.ContainsKey(PROP_OWNER_CONTACT_ID))[PROP_OWNER_CONTACT_ID] = ssnInternalCheckResult;// new Dictionary<string, string> { { PROP_OWNER_CONTACT_ID, ssnInternalCheckResult } };
                await _service.UpdateAsync(submissionProperty.Id, submissionProperty);
            }

            //handle exception

            //return response

            var response = new SubmissionResponse
            {
                result = result.ToString(),
                contactId = contactId
            };

            return response;
        }

        private async Task<string> GetValidTaxId(string ssn, string submissionId, string propertyValue)
        {
            string taxId = ssn;

            if (submissionId == null)
            {
                throw new Exception("submissionId is null.");
            }

            var isRedactedTaxId = ssn.Length == 4 || ssn.Contains(".");
            if (isRedactedTaxId)
            {
                //fetch full tax id using submission id
                //var submissionProperty = await _service.GetBySubmissionId(submissionId);

                //if not valid then set taxid to null
                taxId = int.TryParse(propertyValue, out int id) ? propertyValue : null;
            }

            if (taxId == null)
            {
                throw new Exception($"No valid Tax Id was found for subission id: {submissionId} from SubmissionProperties.");
            }

            return taxId;
        }
    }
}
