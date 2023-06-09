using System.Text;
using EasMe.Logging;
using EasMe.Logging.Internal;
using NewWorldMarket.Core.Abstract;
using NewWorldMarket.Core.Constants;
using NewWorldMarket.Core.Tools;

namespace NewWorldMarket.Business.Services;

public class FileLogger : EasLog,IFileLogger
{
    public FileLogger(string source, string folderName) : base(source, folderName)
    {
    }

    public FileLogger(string source) : base(source)
    {
    }
    public FileLogger() : base("FileLogger")
    {
    }
    public void Log(ActionType actionType, ResultSeverity severity, string message, object? obj = null)
    {
        var level = severity.ToEasLogLevel();
        if (!IsLogLevelEnabled(level)) return;
        var user = SessionLib.This.GetUser();
        var sb = new StringBuilder();
        sb.Append($"[ACTION:{actionType}]");
        if (user is not null)
        {
            sb.Append($" - [USER:{user.Guid}]");
        }
        if (!message.IsNullOrEmpty())
        {
            sb.Append($" - {message}");
        }
        if (obj != null)
        {
            if (obj is string)
            {
                sb.Append($" - {obj}");
            }
            else
            {
                sb.Append($" - {obj.ToJsonString()}");

            }
        }
        WriteLog(_LogSource,level,null,sb.ToString());
    }

    public void Log(ActionType actionType, string message, object? obj = null)
    {
        Log(actionType, ResultSeverity.Info, message, obj);
    }

    public void Log(ActionType actionType, object? obj = null)
    {
        Log(actionType, ResultSeverity.Info, "", obj);
    }
}