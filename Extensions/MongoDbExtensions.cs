using MongoDB.Driver;
using SubmissionsProcessor.API.Models;

namespace SubmissionsProcessor.API.Extensions
{
    public static class MongoDbExtension
    {
        public static IServiceCollection AddMongoDatabase(this IServiceCollection services, IConfiguration config)
        {
            var mongodbConfig = config.GetSection("AvokaDatabase").Get<AvokaDatabaseSettings>();

            if(mongodbConfig is null) { throw new Exception("No mongodb settings in appsettings."); }

            var mongoClient = new MongoClient(mongodbConfig.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongodbConfig.DatabaseName);
            services.AddScoped(provider => mongoDatabase);

            return services;
        }
    }
}

