using SubmissionsProcessor.API.Models;
using SubmissionsProcessor.API.Models.MongoDB;
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

            try { 

                //TODO: Validate SubmissionId AND userId by querying submissions catalogue 

                var submissionProperty = await _service.GetBySubmissionId(submissionId);
                var propertyValue = submissionProperty?.Properties?.FirstOrDefault()?.GetValueOrDefault(PROP_OWNER_TAX_ID);

                var taxId = await GetValidTaxId(model.SSN, submissionId, propertyValue);
                var role = model.Role == "Owner" ? 1 : 0;

                //mocking soap api call here
                var ssnInternalCheckResult = await _ssnCheckMockService.SSNInternalCheckAsync(taxId, role.ToString());
            
            
                //update contactId in db if valid AND role=owner
                if (role == 1 && int.TryParse(ssnInternalCheckResult, out contactId))
                {
                    UpdateDbWithContactId(submissionProperty, contactId);
                }
            }
            catch (Exception ex) {

                _logger.LogError(ex.Message, ex);
                
                //throw again
                throw;

            }


            //return response
            var response = new SubmissionResponse
            {
                result = result.ToString(),
                contactId = contactId
            };

            return response;
        }

        private async Task UpdateDbWithContactId(SubmissionProperty? submissionProperty, int contactId)
        {
            submissionProperty.Properties.FirstOrDefault(x => x.ContainsKey(PROP_OWNER_CONTACT_ID))[PROP_OWNER_CONTACT_ID] = contactId.ToString();
            await _service.UpdateAsync(submissionProperty.Id, submissionProperty);

        }

        private async Task<string> GetValidTaxId(string ssn, string submissionId, string propertyValue)
        {
            string taxId = ssn;

            if (submissionId == null)
            {
                throw new BadHttpRequestException("submissionId is null.");
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
                throw new BadHttpRequestException($"No valid Tax Id was found for subission id: {submissionId} from SubmissionProperties.");
            }

            return taxId;
        }
    }
}
