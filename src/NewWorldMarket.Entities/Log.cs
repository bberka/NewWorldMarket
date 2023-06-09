﻿using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorldMarket.Entities;

public class Log : IEntity
{
    [Key] 
    public Guid Guid { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public Guid? UserGuid { get; set; }
    public bool SuccessStatus { get; set; } = false;
    public int Severity { get; set; } = 0;
    public int ActionType { get; set; }

    [MaxLength(1000)] 
    public string Message { get; set; } = string.Empty;
    [MaxLength(5000)]
    public string? Data { get; set; } 

    [MaxLength(64)]
    public string? RemoteIpAddress { get; set; }
    [MaxLength(64)]
    public string? XRealIpAddress { get; set; }
    [MaxLength(64)]
    public string? XForwardedForIpAddress { get; set; }
    [MaxLength(64)]
    public string? CfConnectingIpAddress { get; set; }
    [MaxLength(258)]
    public string? UserAgent { get; set; }

    //Virtual
    public virtual User? User { get; set; }

}