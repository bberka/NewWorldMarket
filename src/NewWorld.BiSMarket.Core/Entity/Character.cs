using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorld.BiSMarket.Core.Entity;

public class Character : IEntity
{
    [Key]
    public Guid Guid { get; set; }

    public DateTime RegisterDate { get; set; } = DateTime.Now;

    [MaxLength(64)]
    public string Name { get; set; }
    public int Region { get; set; }
    public int Server { get; set; }
    public Guid UserGuid { get; set; }
    public virtual User User { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<OrderRequest> OrderRequests { get; set; } = new List<OrderRequest>();
}