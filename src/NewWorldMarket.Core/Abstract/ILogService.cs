using EasMe.Result;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Core.Abstract;

public interface ILogService
{
    public void Log(ActionType actionType, ResultSeverity severity, string message, object? obj = null);
    /// <summary>
    /// Creates log with Severity.Info
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="message"></param>
    /// <param name="obj"></param>
    public void Log(ActionType actionType,  string message, object? obj = null);
    /// <summary>
    /// Creates log with Severity.Info without message
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="obj"></param>
    public void Log(ActionType actionType, object? obj = null);
    public void Info(ActionType actionType, string message);
    public void Warning(ActionType actionType, string message);
    public void Error(ActionType actionType, string message);
    public void Fatal(ActionType actionType, string message);

}