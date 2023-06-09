using EasMe.Logging;
using EasMe.Result;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Core.Abstract;

/// <summary>
/// File logger that uses IEasLog interface
/// </summary>
public interface IFileLogger : IEasLog
{
    //void LogResult(Result result);
    void Log(ActionType actionType,ResultSeverity severity, string message, object? obj = null);
    /// <summary>
    /// Creates log with Severity.Info
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="message"></param>
    /// <param name="obj"></param>
    void Log(ActionType actionType, string message, object? obj = null);
    /// <summary>
    /// Creates log with Severity.Info without message
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="obj"></param>
    void Log(ActionType actionType, object? obj = null);
}