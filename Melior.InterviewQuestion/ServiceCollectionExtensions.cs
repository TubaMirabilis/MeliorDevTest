using System.Configuration;
using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Melior.InterviewQuestion;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMeliorServices(this IServiceCollection services)
    {
        var sd = ConfigurationManager.AppSettings["DataStoreType"] switch
        {
            "Backup" => new ServiceDescriptor(typeof(IAccountDataStore), typeof(BackupAccountDataStore), ServiceLifetime.Scoped),
            _ => new ServiceDescriptor(typeof(IAccountDataStore), typeof(AccountDataStore), ServiceLifetime.Scoped)
        };
        services.Add(sd);
        services.AddScoped<IPaymentService, PaymentService>();  
        return services;
    }
}
