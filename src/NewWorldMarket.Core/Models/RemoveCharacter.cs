﻿namespace NewWorldMarket.Core.Models;

public class RemoveCharacter
{
    public Guid UserGuid { get; set; }
    public Guid CharacterGuid { get; set; }
}