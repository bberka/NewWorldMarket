using NewWorld.BiSMarket.Core.Constants;

namespace NewWorld.BiSMarket.Core;

public static class ExtensionMethods
{
    public static string ToMessage(this ErrCode errorCode)
    {
        if (ConstMgr.IsDevelopment) return "Error Code: " + errorCode;
        return "An error occurred while processing your request. Error Code: E" + (int)errorCode;
    }
}