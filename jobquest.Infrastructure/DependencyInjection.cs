using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetSection("MongoDbSettings");
        
        Task.Run(async () =>
        {
            await DB.InitAsync(dbConnection["DatabaseName"],
                MongoClientSettings.FromConnectionString(dbConnection["ConnectionString"]));
        }).GetAwaiter().GetResult();
        return services;
    }
}