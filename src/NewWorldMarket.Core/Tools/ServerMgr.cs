using NewWorldMarket.Core.Constants;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Core.Tools;

public class ServerMgr
{
    private static ServerMgr? Instance;

    private ServerMgr()
    {
    }

    public static ServerMgr This
    {
        get
        {
            Instance ??= new ServerMgr();
            return Instance;
        }
    }

    public bool IsValidServer(int regionId, int serverId)
    {
        return ConstMgr.Regions.Any(x => x.Id == regionId && x.Servers.Any(y => y.Id == serverId));
    }

    public bool IsValidServer(int serverId)
    {
        return ConstMgr.Regions.Any(x => x.Servers.Any(y => y.Id == serverId));
    }

    public bool IsValidRegion(int regionId)
    {
        return ConstMgr.Regions.Any(x => x.Id == regionId);
    }

    public Region? GetRegion(int regionId)
    {
        return ConstMgr.Regions.FirstOrDefault(x => x.Id == regionId);
    }

    public Server? GetServer(int serverId)
    {
        return ConstMgr.Regions.SelectMany(x => x.Servers).FirstOrDefault(x => x.Id == serverId);
    }

    public IReadOnlyDictionary<int, string> GetRegionsAsDictionary()
    {
        return ConstMgr.Regions.ToDictionary(x => x.Id, x => x.Name);
    }

    public IReadOnlyDictionary<int, string> GetServersAsDictionary(int regionId)
    {
        var region = GetRegion(regionId);
        if (!region.HasValue) return new Dictionary<int, string>();
        return region.Value.Servers.ToDictionary(x => x.Id, x => x.Name) ?? new Dictionary<int, string>();
    }
}