using System.Configuration;
using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Melior.InterviewQuestion;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMeliorServices(this IServiceCollection services)
    {
        if (ConfigurationManager.AppSettings["DataStoreType"] == "Backup")
        {
            services.AddScoped<IAccountDataStore, BackupAccountDataStore>();
        }
        else
        {
            services.AddScoped<IAccountDataStore, AccountDataStore>();
        }
        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }
}
