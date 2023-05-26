using EasMe.EntityFrameworkCore.V2;
using EasMe.Result;
using NewWorld.BiSMarket.Core.Entity;
using Image = NewWorld.BiSMarket.Core.Entity.Image;

namespace NewWorld.BiSMarket.Core.Abstract;

public interface IUnitOfWork
{
    public IGenericRepository<Order> OrderRepository { get; set; }
    public IGenericRepository<User> UserRepository { get; set; }
    public IGenericRepository<OrderRequest> OrderRequestRepository { get; set; }
    public IGenericRepository<Image> ImageRepository { get; set; }
    public IGenericRepository<Character> CharacterRepository { get; set; }
    public Result Save();
}