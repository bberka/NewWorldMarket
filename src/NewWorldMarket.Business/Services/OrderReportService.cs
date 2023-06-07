using NewWorldMarket.Core;
using NewWorldMarket.Core.Abstract;
using NewWorldMarket.Core.Constants;
using NewWorldMarket.Core.Entity;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Business.Services;

public class OrderReportService : IOrderReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Result CreateReport(Guid? userGuid, CreateOrderReport request)
    {
        
        if (userGuid is not null)
        {
            var userExists = _unitOfWork.UserRepository.Any(x => x.Guid == userGuid);
            if (!userExists)
                return DomainResult.User.ErrNotFound;
        }

        
        var orderExists = _unitOfWork.OrderRepository.Any(x => x.Guid == request.OrderGuid);
        if (!orderExists)
            return DomainResult.Order.ErrNotFound;
        //var validReportType = Enum.IsDefined(typeof(OrderReportType), request.Type);
        //if (!validReportType)
        //    return DomainResult.InvalidRequest;
        var orderReport = new OrderReport
        {
            Guid = Guid.NewGuid(),
            UserGuid = userGuid,
            OrderGuid = request.OrderGuid,
            Message = request.Message,
            Type = (int)OrderReportType.None,
            State = (int)OrderReportState.Pending,
        };
        _unitOfWork.OrderReportRepository.Insert(orderReport);
        var result = _unitOfWork.Save();
        return result;
    }
}