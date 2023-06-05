using EasMe.EntityFrameworkCore.V2;
using EasMe.Result;
using NewWorldMarket.Core;
using NewWorldMarket.Core.Abstract;
using NewWorldMarket.Core.Constants;
using NewWorldMarket.Core.Entity;

namespace NewWorldMarket.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly MarketDbContext _context;

    public UnitOfWork()
    {
        _context = new MarketDbContext();
        OrderRepository = new GenericRepository<Order, MarketDbContext>(_context);
        UserRepository = new GenericRepository<User, MarketDbContext>(_context);
        OrderCompleteRequestRepository = new GenericRepository<OrderRequest, MarketDbContext>(_context);
        ImageRepository = new GenericRepository<Image, MarketDbContext>(_context);
        CharacterRepository = new GenericRepository<Character, MarketDbContext>(_context);
    }

    public IGenericRepository<OrderRequest> OrderCompleteRequestRepository { get; set; }
    public IGenericRepository<Order> OrderRepository { get; set; }
    public IGenericRepository<User> UserRepository { get; set; }
    public IGenericRepository<OrderRequest> OrderRequestRepository { get; set; }
    public IGenericRepository<Image> ImageRepository { get; set; }
    public IGenericRepository<Character> CharacterRepository { get; set; }

    public Result Save()
    {
        try
        {
            var result = _context.SaveChanges();
            if (result > 0)
                return Result.Success();
            return Result.Fatal(ErrCode.InternalDbError.ToMessage());
        }
        catch (Exception ex)
        {
            //Todo: Logging

            return Result.Fatal(ErrCode.InternalDbError.ToMessage());
        }
        finally
        {
            _context.Dispose();
        }
    }
}