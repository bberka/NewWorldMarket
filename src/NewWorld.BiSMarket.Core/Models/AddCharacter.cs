namespace NewWorld.BiSMarket.Core.Models;

public class AddCharacter
{
    public int Region { get; set; }
    public int Server { get; set; }
    public string Name { get; set; }
    public Guid UserGuid { get; set; }
}