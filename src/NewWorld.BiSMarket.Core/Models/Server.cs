namespace NewWorld.BiSMarket.Core.Models;

public readonly struct Server
{
    public Server(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }
    public string Name { get; init; }
}