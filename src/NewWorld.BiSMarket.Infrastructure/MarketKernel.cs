using Microsoft.EntityFrameworkCore;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Infrastructure;
using NewWorld.BiSMarket.Infrastructure.Services;
using Ninject;

namespace NewWorld.BiSMarket.Core;

public class MarketKernel  : StandardKernel
{

	private MarketKernel() { }
	public static MarketKernel This
	{
		get
		{
			Instance ??= new();
			return Instance;
		}
	}
	private static MarketKernel? Instance;

    public void Initialize()
    {
        Bind<IUnitOfWork>().To<UnitOfWork>();
        Bind<DbContext>().To<MarketDbContext>();

        Bind<IOrderService>().To<OrderService>();
		Bind<IUserService>().To<UserService>();
    }

    public T GetInstance<T>()
    {
        return KernelInstance.Get<T>();
    }


}