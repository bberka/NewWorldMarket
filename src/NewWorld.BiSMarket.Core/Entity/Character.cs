using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorld.BiSMarket.Core.Entity;

public class Character : IEntity
{
    [Key]
    public Guid Guid { get; set; }

    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime? DeletedDate { get; set; } 

    [MaxLength(64,ErrorMessage = "Name can not be longer than 64 characters")]
    public string Name { get; set; }
    public int Region { get; set; }

    [Display(Name = "World")]
    public int Server { get; set; }
    public Guid UserGuid { get; set; }
    public virtual User User { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<OrderRequest> OrderRequests { get; set; } = new List<OrderRequest>();
}