using EasMe.Extensions;
using Grpc.Net.Client.Balancer;
using Newtonsoft.Json;

namespace NewWorldMarket.Core.Tools;

public static class Serializer
{
    public static string ToJson(object obj)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        var json = JsonConvert.SerializeObject(obj, settings);
        return json.RemoveLineEndings();
    }
}