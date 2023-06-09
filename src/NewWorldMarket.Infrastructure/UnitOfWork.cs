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
        OrderReportRepository = new GenericRepository<OrderReport, MarketDbContext>(_context);
        LogRepository = new GenericRepository<Log, MarketDbContext>(_context);
        BlockedIpAddressRepository = new GenericRepository<BlockedIpAddress, MarketDbContext>(_context);
        BlockedUserRepository = new GenericRepository<BlockedUser, MarketDbContext>(_context);
        EmailVerificationTokenRepository = new GenericRepository<EmailVerificationToken, MarketDbContext>(_context);
        ResetPasswordTokenRepository = new GenericRepository<ResetPasswordToken, MarketDbContext>(_context);
    }

    public IGenericRepository<OrderRequest> OrderCompleteRequestRepository { get; set; }
    public IGenericRepository<Order> OrderRepository { get; set; }
    public IGenericRepository<User> UserRepository { get; set; }
    public IGenericRepository<OrderRequest> OrderRequestRepository { get; set; }
    public IGenericRepository<Image> ImageRepository { get; set; }
    public IGenericRepository<Character> CharacterRepository { get; set; }
    public IGenericRepository<OrderReport> OrderReportRepository { get; set; }
    public IGenericRepository<Log> LogRepository { get; set; }
    public IGenericRepository<BlockedIpAddress> BlockedIpAddressRepository { get; set; }
    public IGenericRepository<BlockedUser> BlockedUserRepository { get; set; }
    public IGenericRepository<EmailVerificationToken> EmailVerificationTokenRepository { get; set; }
    public IGenericRepository<ResetPasswordToken> ResetPasswordTokenRepository { get; set; }

    /// <summary>
    /// Saves changes to database in a transaction scope. If any error occurs, it will be rolled back. After saving changes, the context will be disposed and can not be used again.
    /// </summary>
    /// <returns><see cref="EasMe.Result.Result"/></returns>
    public Result Save()
    {
        var transaction = _context.Database.BeginTransaction();
        try
        {
            var result = _context.SaveChanges();
            if (result > 0)
            {
                transaction.Commit();
                return Result.Success();
            }
            transaction.Rollback();
            return Result.Fatal(ErrCode.InternalDbError.ToMessage());
        }
        catch (Exception ex)
        {
            //Todo: Logging
            transaction.Rollback();
            return Result.Fatal(ErrCode.InternalDbError.ToMessage());
        }
        finally
        {
            transaction.Dispose();
            _context.Dispose();
        }
    }
}