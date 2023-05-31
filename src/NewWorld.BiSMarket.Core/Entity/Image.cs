using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorld.BiSMarket.Core.Entity;

public class Image : IEntity
{
    [Key]
    public Guid Guid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public byte[] Bytes { get; set; }
    public byte[] SmallIconBytes { get; set; }
    public string OcrTextResult { get; set; } = string.Empty;
    public string OcrItemDataResult { get; set; } = string.Empty;
    public Guid UserGuid { get; set; }
    public User User { get; set; }

}