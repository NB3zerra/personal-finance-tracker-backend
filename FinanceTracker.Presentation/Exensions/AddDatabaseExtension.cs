using FinanceTracker.Infra.Interfaces;
using FinanceTracker.Infra.Repositories;
using FinanceTracker.Infra.Settings;
using MongoDB.Driver;

namespace FinanceTracker.Presentation.Exensions
{
    public static class AddDatabaseExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(configuration.GetConnectionString("DefaultConnection")));
            
            services.AddScoped(sp =>
                sp.GetRequiredService<IMongoClient>().GetDatabase("FinanceTracker"));

            
            
            return services;
        }
    }
}