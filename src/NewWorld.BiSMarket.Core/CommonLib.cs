namespace NewWorld.BiSMarket.Core;

public static class CommonLib
{
    public static string ConvertPriceToReadableString(this float price)
    {
        if (price < 1000)
        {
            return $"{price:0.00}";
        }
        else if (price < 1000000)
        {
            return $"{price / 1000:0.00}k";
        }
        else if (price < 1000000000)
        {
            return $"{price / 1000000:0.00}m";
        }
        else
        {
            return $"{price / 1000000000:0.00}b";
        }

    }
}