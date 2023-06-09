using EasMe.EntityFrameworkCore.V2;
using EasMe.Result;
using NewWorldMarket.Core.Entity;

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

    public Result Save();
}