namespace NewWorld.BiSMarket.Core.Models;

public class OcrReadResult
{
    public byte[] FullImageBytes { get; set; }
    public byte[] IconBytes { get; set; }
    public string OcrTextResult { get; set; }
    public List<string> Errors { get; set; }
    public ItemV3 Item { get; set; }

}