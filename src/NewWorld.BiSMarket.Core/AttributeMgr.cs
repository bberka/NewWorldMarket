using System.Text;
using EasMe.Extensions;
using NewWorld.BiSMarket.Core.Constants;

namespace NewWorld.BiSMarket.Core;

public class AttributeMgr
{
    private static AttributeMgr? Instance;
    public IReadOnlyDictionary<int, string> AttributeList;
    public IReadOnlyDictionary<string, string> AttributeShortList;

    private AttributeMgr()
    {
        AttributeList = Enum.GetValues(typeof(AttributeType))
            .Cast<AttributeType>()
            .ToDictionary(t => (int)t, t => t.ToString());
        var attributeShortList = new Dictionary<string, string>();
        foreach (var item in AttributeList)
        {
            var first3char = item.Value.Substring(0, 3);
            attributeShortList.Add(item.Value, first3char);
        }

        AttributeShortList = attributeShortList;
    }

    public static AttributeMgr This
    {
        get
        {
            Instance ??= new AttributeMgr();
            return Instance;
        }
    }

    public string ParseAttributeFormattedTextHtmlRaw(string attributeText)
    {
        if (attributeText.IsNullOrEmpty()) return "Could not be read";

        var split = attributeText.Split(",");

        var sb = new StringBuilder();
        foreach (var s in split)
        {
            var attributeType = int.Parse(s);
            if (attributeType < 0) continue;
            var typeName = AttributeList[attributeType];
            sb.Append(typeName);
            sb.Append("<br/>");
        }

        return sb.ToString();
    }

    public string ParseAttributeFormattedText(string attributeText)
    {
        if (attributeText.IsNullOrEmpty()) return "Could not be read";

        var split = attributeText.Split(",");

        var sb = new StringBuilder();
        foreach (var s in split)
        {
            var attributeType = int.Parse(s);
            if (attributeType < 0) continue;
            var typeName = AttributeList[attributeType];
            sb.Append(typeName);
            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }

    public bool IsValid(int attributeType)
    {
        return AttributeList.ContainsKey(attributeType);
    }
}