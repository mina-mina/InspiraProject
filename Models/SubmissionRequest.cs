using GoalSeek.API.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SubmissionsProcessor.API.Models
{
    public class SubmissionRequest
    {
        [SSNValidator]
        public string SSN { get; set; }
        public string? Role { get; set; }
        
    }
}
