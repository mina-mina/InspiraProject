﻿using SubmissionsProcessor.API.Models;
using SubmissionsProcessor.API.Models.MongoDB;
using System.Net;
using System.Text.Json;

namespace SubmissionsProcessor.API.Middlewares
{
    public class SubmissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SubmissionMiddleware> _logger;
        private const string _requestSubmissionIdKey = "_submissionIdKey";
        private const string _requestUserIdKey = "_userIdKey";
        private object? _correlationId;

        public SubmissionMiddleware(RequestDelegate next, ILogger<SubmissionMiddleware> logger)
        {

            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                ValidateHttpContextItems(context);

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                await HandleErrorResponse(ex, context);
            }
        }

        private void ValidateHttpContextItems(HttpContext context)
        {
            //we assume the submissionId is being passed in HttpContext Items
            context.Items.TryGetValue(_requestSubmissionIdKey, out object? submissionId);

            //if request is authenticated using oauth2 then userid should come from HttpContext.User.Identity
            //below line is to emulate above
            context.Items.TryGetValue(_requestUserIdKey, out object? userId);

            if (submissionId is null)
            {
                throw new KeyNotFoundException("No SubmissionId found the request.");
            }
            if (userId is null)
            {
                throw new UnauthorizedAccessException("No UserId found in the request. Unauthorized request.");
            }

            SetSubmissionContext(context, submissionId);

        }

        private void SetSubmissionContext(HttpContext context, object submissionId)
        {
            //we get correlationId here for logging in case things go wrong in the middleware
            context.Items.TryGetValue(_requestUserIdKey, out _correlationId);

            ISubmissionContext submissionContext = context.RequestServices.GetRequiredService<ISubmissionContext>();

            //set submissionObj for later use
            submissionContext.SubmissionId = submissionId?.ToString();
            submissionContext.CorrelationId = _correlationId?.ToString();
        }

        private Task HandleErrorResponse(Exception ex, HttpContext context)
        {
            var correlationId= _correlationId?.ToString() ?? context.Request?.HttpContext?.TraceIdentifier;
            var errMsg = $"{typeof(SubmissionMiddleware).Name} Middleware Error. {ex.Message}- CorrelationId: {_correlationId}.";

            var statusCode = (int)HttpStatusCode.InternalServerError;

            var response = new SubmissionResponse
            {
                result = "error",
            };
            if (ex is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            if (ex is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
            }
            _logger.LogError($"{errMsg} Response statuscode returned:{statusCode}", ex.GetType().Name, ex);

            var result = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
