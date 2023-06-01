using System.Text;
using NewWorld.BiSMarket.Core.Constants;

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

    public static string GetOrderTypeString(int type)
    {
        var enumType = (OrderType)type;
        return enumType switch
        {
            OrderType.Buy => "Buy",
            OrderType.Sell => "Sell",
            _ => "Unknown"
        };

    }

    public static string RemoveSpecialCharacters(this string str,string allowedSpecialChars = "")
    {
        var sb = new StringBuilder();
        foreach (var c in str)
        {
            if (char.IsLetterOrDigit(c) || allowedSpecialChars.Contains(c) || char.IsWhiteSpace(c))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static string RemoveNumbers(this string str)
    {
        var sb = new StringBuilder();
        foreach (var c in str)
        {
            if (!char.IsDigit(c))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
    public static string RemoveWhitespace(this string str)
    {
        var sb = new StringBuilder();
        foreach (var c in str)
        {
            if (!char.IsWhiteSpace(c))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static string ConvertDateDiffAsReadableText(DateTime date1, DateTime date2)
    {
        var diff = date1 - date2;
        if (diff.TotalDays > 1)
        {
            return $"{diff.TotalDays:0} days";
        }
        else if (diff.TotalHours > 1)
        {
            return $"{diff.TotalHours:0} hours";
        }
        else if (diff.TotalMinutes > 1)
        {
            return $"{diff.TotalMinutes:0} minutes";
        }
        else
        {
            return $"{diff.TotalSeconds:0} seconds";
        }

    }
}