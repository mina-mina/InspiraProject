
using Microsoft.Extensions.DependencyInjection;
using SubmissionsProcessor.API.Controllers;
using SubmissionsProcessor.API.Extensions;
using SubmissionsProcessor.API.Middlewares;
using SubmissionsProcessor.API.Models;
using SubmissionsProcessor.API.Repositories;
using SubmissionsProcessor.API.Services;
using SubmissionsProcessor.API.Services.MongoDB;

namespace InpiraProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMongoDatabase(builder.Configuration);
            //builder.Services.AddScoped<SubmissionsService>();
            //builder.Services.AddScoped<FormsService>();
            builder.Services.AddScoped<ISubmissionPropertiesService, SubmissionPropertiesService>();
            builder.Services.AddScoped<ISubmissionContext, SubmissionContext>();
            builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            builder.Services.AddScoped<ISSNInternalCheckMockService, SSNInternalCheckMockService>();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseWhen(context => context.Request.Path.StartsWithSegments($"/{typeof(SubmissionController).Name}", StringComparison.OrdinalIgnoreCase), appBuilder =>
            {
                app.UseMiddleware<SumbmissionMiddleware>();
            });
            app.MapControllers();

            app.Run();
        }
    }
}
