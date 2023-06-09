using EasMe.EntityFrameworkCore.V2;
using EasMe.Result;
using NewWorldMarket.Entities;

namespace NewWorldMarket.Core.Abstract;

public interface IUnitOfWork
{
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

    public Result Save();
}