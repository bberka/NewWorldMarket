namespace NewWorld.BiSMarket.Core.Models;

public class ItemOcrResult
{
    private ItemOcrResult()
    {
        
    }
    public static ItemOcrResult Fail(params string[] errors)
    {
        return new ItemOcrResult
        {
            IsSuccess = false,
            Errors = errors
        };
    }

    public static ItemOcrResult SuccessWithErrors(ItemImageData image, ItemV3 readData, Dictionary<int, string> pages,
        params string[] errors)
    {
        if (errors.Length > 4)
        {
            return Fail(errors);
        }
        return new ItemOcrResult
        {
            IsSuccess = true,
            ItemImageData = image,
            ItemOcrReadData = readData,
            Pages = pages,
            Errors = errors
        };
    }
    public static ItemOcrResult Success(ItemImageData image, ItemV3 readData, Dictionary<int, string> pages)
    {
        return new ItemOcrResult
        {
            IsSuccess = true,
            ItemImageData = image,
            ItemOcrReadData = readData,
            Pages = pages,
        };
    }

    public bool IsSuccess { get; init; } 
    public IReadOnlyCollection<string> Errors { get; init; }
    public ItemImageData ItemImageData { get; init; }
    public ItemV3 ItemOcrReadData { get; init; }
    public Dictionary<int,string> Pages { get; init; }
}