using EasMe.Logging;
using NewWorldMarket.Core;
using NewWorldMarket.Core.Abstract;
using NewWorldMarket.Core.Constants;
using NewWorldMarket.Core.Tools;
using NewWorldMarket.Entities;

namespace NewWorldMarket.Business.Services;

/// <summary>
/// Database log service
/// </summary>
public class LogService : ILogService
{
    private readonly IUnitOfWork _unitOfWork;
    private static readonly IEasLog _logger = EasLogFactory.CreateLogger();

    /// <summary>
    /// Queue task manager 
    /// </summary>
    private static readonly EasTask _taskManager = new EasTask();
    public LogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
  

    public void Log(ActionType actionType, ResultSeverity severity, string message, object? obj = null)
    {
        var context = HttpContextHelper.Current;
        var requestInfo = context?.GetInfo();
        var user = SessionLib.This.GetUser();
        var action = new Action(() =>
        {
            try
            {
                var log = new Log
                {
                    ActionType = (int)actionType,
                    Message = message,
                    SuccessStatus = severity == ResultSeverity.Info,
                    RegisterDate = DateTime.Now,
                    Severity = (int)severity,
                    CfConnectingIpAddress = requestInfo?.CfConnectingIpAddress,
                    RemoteIpAddress = requestInfo?.RemoteIpAddress,
                    UserAgent = requestInfo?.UserAgent,
                    UserGuid = user?.Guid,
                    XForwardedForIpAddress = requestInfo?.XForwardedForIpAddress,
                    XRealIpAddress = requestInfo?.XRealIpAddress,
                    Data = obj.ToJsonString()
                };
                _unitOfWork.LogRepository.Insert(log);
                var dbSaveResult = _unitOfWork.Save();
                if (dbSaveResult.IsFailure)
                {
                    _logger.Error($"LogService.Log: {dbSaveResult.ErrorCode}");
                }

            }
            catch (Exception ex)
            {
                _logger.Exception(ex, "LogService.Log");
            }
        });
        _taskManager.AddToQueue(action);

    }

    public void Log(ActionType actionType, string message, object? obj = null)
    {
        Log(actionType,ResultSeverity.Info,message,obj);
    }

    public void Log(ActionType actionType, object? obj = null)
    {
        Log(actionType, ResultSeverity.Info, "-", obj);
    }

    public void Info(ActionType actionType, string message)
    {
        Log(actionType , ResultSeverity.Info,message);
    }

    public void Warning(ActionType actionType, string message)
    {
        Log(actionType, ResultSeverity.Warn, message);
        
    }

    public void Error(ActionType actionType, string message)
    {
        Log(actionType, ResultSeverity.Error, message);
        
    }

    public void Fatal(ActionType actionType, string message)
    {
        Log(actionType, ResultSeverity.Fatal, message);
    }
}