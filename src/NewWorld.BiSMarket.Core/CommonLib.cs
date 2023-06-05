using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Core;

public static class CommonLib
{
    public static string ConvertPriceToReadableString(this float price)
    {
        if (price < 1000)
            return $"{price:0.00}";
        if (price < 1000000)
            return $"{price / 1000:0.00}k";
        if (price < 1000000000)
            return $"{price / 1000000:0.00}m";
        return $"{price / 1000000000:0.00}b";
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

    public static string RemoveSpecialCharacters(this string str, string allowedSpecialChars = "")
    {
        var sb = new StringBuilder();
        foreach (var c in str)
            if (char.IsLetterOrDigit(c) || allowedSpecialChars.Contains(c) || char.IsWhiteSpace(c))
                sb.Append(c);
        return sb.ToString();
    }

    public static string RemoveNumbers(this string str)
    {
        var sb = new StringBuilder();
        foreach (var c in str)
            if (!char.IsDigit(c))
                sb.Append(c);
        return sb.ToString();
    }

    public static string RemoveLetters(this string str)
    {
        var sb = new StringBuilder();
        foreach (var c in str)
            if (!char.IsLetter(c))
                sb.Append(c);
        return sb.ToString();
    }

    public static string RemoveWhitespace(this string str)
    {
        var sb = new StringBuilder();
        foreach (var c in str)
            if (!char.IsWhiteSpace(c))
                sb.Append(c);
        return sb.ToString();
    }

    public static string ConvertDateDiffAsReadableText(DateTime date1, DateTime date2)
    {
        var diff = date1 - date2;
        if (diff.TotalDays > 1)
            return $"{diff.TotalDays:0} days";
        if (diff.TotalHours > 1)
            return $"{diff.TotalHours:0} hours";
        if (diff.TotalMinutes > 1)
            return $"{diff.TotalMinutes:0} minutes";
        return $"{diff.TotalSeconds:0} seconds";
    }

    public static string SeparateWordsAndRemoveIfFirstWordIsShort(this string text, byte checkNum)
    {
        var split = text.Split(' ');
        var first = split[0];
        if (first.Length < checkNum) return text.Replace(first, "");
        return text;
    }

    public static string FixGemNameText(this string text)
    {
        var split = text.Split(' ');
        var last = split[^1];
        var fixedLast = last
                .Replace("I", "")
                .Replace("II", "")
                .Replace("III", "")
                .Replace("IV", "")
                .Replace("l", "")
                .Replace("ll", "")
                .Replace("lll", "")
                .Replace("lV", "")
            ;
        return text.Replace(last, fixedLast);
    }

    public static byte[] ResizeImageWidth(byte[] imageBytes, int newWidth, out int imageHeight)
    {
        // Create a MemoryStream from the image byte array
        using var imageStream = new MemoryStream(imageBytes);
        // Create an Image object from the MemoryStream
        using var originalImage = Image.FromStream(imageStream);
        // Get the original width and height of the image
        var width = originalImage.Width;
        var height = originalImage.Height;

        // Calculate the aspect ratio of the image
        var aspectRatio = (double)width / height;

        // Calculate the new height based on the new width and aspect ratio
        imageHeight = (int)(newWidth / aspectRatio);

        // Create a new Bitmap object with the new dimensions
        using var resizedImage = new Bitmap(newWidth, imageHeight);
        // Create a Graphics object from the resized image
        using var graphics = Graphics.FromImage(resizedImage);
        // Set the interpolation mode to high quality bicubic
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

        // Draw the original image onto the resized image
        graphics.DrawImage(originalImage, 0, 0, newWidth, imageHeight);

        // Create a MemoryStream to store the resized image
        using var resizedImageStream = new MemoryStream();
        // Save the resized image to the MemoryStream in JPEG format
        resizedImage.Save(resizedImageStream, ImageFormat.Jpeg);

        // Get the resized image data as a byte array
        var resizedImageBytes = resizedImageStream.ToArray();

        return resizedImageBytes;
    }
}