using jobquest.Infrastructure.Services.Files;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Entities;

namespace jobquest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetSection("MongoDbSettings");
        var connectionString = dbConnection["ConnectionString"];
        var databaseName = dbConnection["DatabaseName"];

        // Initialize MongoDB using MongoClient and register it in DI container
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseName);

        // Register MongoDB database as a singleton (so it's shared across the app)
        services.AddSingleton<IMongoDatabase>(mongoDatabase);

        // Register GridFSBucket for file storage (as a singleton)
        services.AddSingleton<IGridFSBucket>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            return new GridFSBucket(database);
        });

        // Register the FileService that interacts with GridFS
        services.AddScoped<FileService>(); // If you have an IFileService interface, you can add it here instead of FileService
        
        Task.Run(async () =>
        {
            await DB.InitAsync(dbConnection["DatabaseName"],
                MongoClientSettings.FromConnectionString(dbConnection["ConnectionString"]));
        }).GetAwaiter().GetResult();
        return services;

        return services;
    }
}