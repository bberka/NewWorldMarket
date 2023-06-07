namespace NewWorldMarket.Web;

public static class Bindings
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddDbContext<MarketDbContext>();
        return serviceCollection;
    }

    public static IServiceCollection AddBusinessDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IImageService, ImageService>();
        serviceCollection.AddScoped<IOrderService, OrderService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IOrderReportService, OrderReportService>();
        return serviceCollection;
    }
}