using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubmissionsProcessor.API.Models;
using SubmissionsProcessor.API.Repositories;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;

namespace SubmissionsProcessor.API.Controllers
{
    //[Authorize] TODO: activate once authentication implemented
    //TODO: add API version
    [ApiController]
    [Route("[controller]")] //[MINA] - should not be called controller - bad name. Maybe submission is more appropriate?
    public class SubmissionController : ControllerBase
    {


        private readonly ISubmissionRepository _submissionRepository;
        private readonly ISubmissionContext _submissionContext;
        private readonly ILogger<SubmissionController> _logger;

        public SubmissionController(ISubmissionRepository submissionRepository, ISubmissionContext submissionContext, ILogger<SubmissionController> logger)
        {
            _submissionRepository = submissionRepository;
            _submissionContext = submissionContext;
            _logger = logger;
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        //[MINA] - bad method naming - maybe Get
        public async Task<IActionResult> Post([FromForm] SubmissionRequest model)
        {
            SubmissionResponse response =null;
            try
            {
                //[MINA] - bit sloppy - no validation?
                //[MINA] security considerations?
                //[MINA] - sanitization?
                // Sending
                response = await _submissionRepository.GetSubmissionResponse(model, _submissionContext.SubmissionId);
            }
            catch (Exception ex)
            {
                //return HandleException(ex);
            }
            return Ok(response);
        }
    }
}
