
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

            //TODO: configure AddAuthorization and AddAuthentication services


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

            //TODO: configure AddApiVersioning service
            //TODO: if prod has swagger then setup swagger versioning and Security
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //TODO: configure UseForwardedHeaders 
            //TODO: add UseSwagger & UseSwaggerUI for prod version setup

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                //TODO: configure API version for swaggerUI
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            //TODO: add UseAuthentication
            app.UseWhen(context => context.Request.Path.StartsWithSegments($"/{typeof(SubmissionController).Name}", StringComparison.OrdinalIgnoreCase), appBuilder =>
            {
                app.UseMiddleware<SubmissionMiddleware>();
            });
            app.MapControllers();

            app.Run();
        }
    }
}
