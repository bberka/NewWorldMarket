using System.ComponentModel.DataAnnotations;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Core.Models;

public class CreateOrderReport
{
    public Guid OrderGuid { get; set; }
    [MaxLength(1000,ErrorMessage = "Reason can not be longer than 1000 characters.")]
    [MinLength(10,ErrorMessage = "Reason can not be shorter than 10 characters.")]
    public string Message { get; set; }
    //public int Type { get; set; } = (int)OrderReportType.None;
}
