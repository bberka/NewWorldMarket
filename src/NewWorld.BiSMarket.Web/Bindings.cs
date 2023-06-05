using NewWorld.BiSMarket.Business.Services;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Infrastructure;

namespace NewWorld.BiSMarket.Web;

public static class Bindings
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddDbContext<MarketDbContext>();
        serviceCollection.AddScoped<IImageService, ImageService>();
        serviceCollection.AddScoped<IOrderService, OrderService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        return serviceCollection;
    }

    public static IServiceCollection AddBusinessDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IImageService, ImageService>();
        serviceCollection.AddScoped<IOrderService, OrderService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        return serviceCollection;
    }
}