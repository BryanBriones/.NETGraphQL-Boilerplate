using Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schema.AutoMapper;
using Schema.Queries;

namespace Schema;

public static class StartupExtensions
{
    public static IServiceCollection AddSchemaLayer(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddPooledDbContextFactory<PharmaservAdminContext>(options => options.UseSqlServer(configuration.GetConnectionString("PharmaservAdmin")));
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
