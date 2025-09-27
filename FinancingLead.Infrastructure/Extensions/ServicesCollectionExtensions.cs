
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FinancingLead.Infrastructure.Repositories;
using FinancingLead.Application.Interfaces;
using FinancingLead.Infrastructure.Services;
using FinancingLead.Infrastructure.Dbcontext;

namespace FinancingLead.Infrastructure.Extensions;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IFinancingLeadRepository, FinancingLeadRepository>();

        services.AddScoped<INotificationClient, FirebaseNotificationClient>();
        return services;
    }
}
