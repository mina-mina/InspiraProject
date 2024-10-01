using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SubmissionsProcessor.API.Models;
using SubmissionsProcessor.API.Repositories;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public string correlationId { get; set; }

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
                return HandleException(ex);
            }
            return Ok(response);
        }

        #region private methods
        private ActionResult HandleException(Exception ex)
        {
            var response = new SubmissionResponse
            {
                result = "error",
                contactId = 0
            };

            if (ex.GetType().Equals(typeof(BadHttpRequestException)))
            {
                _logger.LogError($"SubmissionController.Post correlationId: {Request?.HttpContext?.TraceIdentifier}, ApplicationException", ex);

                return BadRequest(response);
            }
            else
                _logger.LogError($"SubmissionController.Post correlationId: {Request?.HttpContext?.TraceIdentifier}, Error Message: {ex.Message}, Stacktrace: {ex.StackTrace}", ex);


            return StatusCode(500, response);
        }
        //USE below for manual request model logging
        private void LogModelStateErrors(ModelStateDictionary modelState)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };

            var errors = JsonSerializer.Serialize(modelState.ToDictionary(x => x.Key, x => x.Value?.Errors), options);
            _logger.LogInformation($"SubmissionController.Post SubmissionRequest correlationId:{Request?.HttpContext?.TraceIdentifier}, Model Validation Errors: {errors}");
        }
        #endregion  
    }
}
