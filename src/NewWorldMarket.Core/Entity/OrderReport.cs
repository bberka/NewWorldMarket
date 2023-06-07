using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Core.Entity;

public class OrderReport : IEntity
{
    [Key]
    public Guid Guid { get; set; }
    public Guid OrderGuid { get; set; }
    public Order Order { get; set; } = null!;
    public Guid? UserGuid { get; set; }
    public User? User { get; set; } = null!;
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    [MaxLength(1000)]
    public string Message { get; set; }
    public int State { get; set; } = (int)OrderReportState.Pending;
    public int Type { get; set; } 

}