using MicroBoss.Application.Common;
using MicroBoss.Domain.Interfaces;
using MicroBoss.Infrastructure.Data;
using MicroBoss.Infrastructure.Data.Repositories;
using MicroBoss.Infrastructure.Data.Services;
using MicroBoss.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBoss.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MicroBossDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentitySetup();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ISequenceGenerator, SequenceGenerator>();
        return services;
    }
}
