namespace NewWorld.BiSMarket.Core.Models;

public readonly struct Region
{
    public Region(int id, string name,params Server[] servers)
    {
        Id = id;
        Name = name;
        Servers = servers;
    }
    public int Id { get; init; }
    public string Name { get; init; }
    public IReadOnlyCollection<Server> Servers { get; init; }
}