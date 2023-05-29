﻿using Microsoft.EntityFrameworkCore;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Infrastructure;
using NewWorld.BiSMarket.Infrastructure.Services;
using Ninject.Modules;

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