using System.ComponentModel.DataAnnotations;

namespace NewWorld.BiSMarket.Core.Models;

public class AddCharacter
{
    public int Region { get; set; }
    [Display(Name = "World")]
    public int Server { get; set; }
    [MaxLength(64, ErrorMessage = "Name can not be longer than 64 characters")]
    public string Name { get; set; }
    public Guid UserGuid { get; set; }
}