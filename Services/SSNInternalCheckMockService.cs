namespace SubmissionsProcessor.API.Services
{
    public class SSNInternalCheckMockService : ISSNInternalCheckMockService
    {
        private readonly ILogger<SSNInternalCheckMockService> _logger;
        private readonly int _overrideRandomContactId;

        public SSNInternalCheckMockService(ILogger<SSNInternalCheckMockService> logger, int overrideRandomContactId = 0)
        {
            _logger = logger;

            //for test purposes
            _overrideRandomContactId = overrideRandomContactId;
        }
        public int RandomContactId { get => _overrideRandomContactId > 0 ? _overrideRandomContactId : getRandomNumber(); }

        public async Task<string> SSNInternalCheckAsync(string ssn, string role)
        {
            return RandomContactId.ToString();
        }
        private int getRandomNumber()
        {
            Random r = new Random();
            int rInt = r.Next(0, 9999999);
            return rInt;
        }
    }
}
