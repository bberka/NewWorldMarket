using EasMe.Result;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Core.Abstract;

public interface IOrderReportService
{
    Result CreateReport(Guid? userGuid, CreateOrderReport request,RequestInformation information);
}