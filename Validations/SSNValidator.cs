using System.ComponentModel.DataAnnotations;

namespace GoalSeek.API.Validations
{
    public class SSNValidator : ValidationAttribute
    {
        bool result = false;

        public override bool IsValid(object value)
        {
            var ssn = value?.ToString();
            if (string.IsNullOrEmpty(ssn))
            {
                base.ErrorMessage = "No SSN provided.";
                return result;
            }
            try
            {
                //perform ssn validation and set result
                ValidateSSN(ssn.Trim());
            }
            catch (Exception ex)
            {
                var msg = "Error validating SSN.";
                base.ErrorMessage = msg;
                result = false;
            }

            return result;
        }

        private void ValidateSSN(string ssn)
        {
            if (ssn.Length == 9 || ssn.Length==4)
            {
                result = int.TryParse(ssn, out var id);
            }
            else if(ssn.Contains("."))
            {
                var redactedTaxId = ssn.Substring(ssn.LastIndexOf('.') + 1);
                result = int.TryParse(redactedTaxId, out var id);
            }
        }
    }
}
