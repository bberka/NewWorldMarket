using System.Text;
using EasMe.Extensions;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Core.Tools;

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

    //public string ParseAttributeFormattedTextHtmlRaw(string attributeText)
    //{
    //    if (attributeText.IsNullOrEmpty()) return "Could not be read";

    //    var split = attributeText.Split(",");

    //    var sb = new StringBuilder();
    //    foreach (var s in split)
    //    {
    //        var attributeType = int.Parse(s);
    //        if (attributeType < 0) continue;
    //        var typeName = AttributeList[attributeType];
    //        sb.Append(typeName);
    //        sb.Append("<br/>");
    //    }

    //    return sb.ToString();
    //}

    public string GetAsViewText(int id, int id2,string separator)
    {
        if (id == id2) id2 = -1;
        if (id < 0 && id2 < 0) return "Could not be read";
        var sb = new StringBuilder();
        if (id >= 0)
        {
            var typeName1 = AttributeList[id];
            sb.Append(typeName1);
        }

        if (id2 >= 0)
        {
            var typeName2 = AttributeList[id2];
            sb.Append("<");
            sb.Append(typeName2);
        }

        return sb.ToString().Trim('<').Replace("<", separator);
    }

    //public string ParseAttributeFormattedText(string attributeText)
    //{
    //    if (attributeText.IsNullOrEmpty()) return "Could not be read";

    //    var split = attributeText.Split(",");

    //    var sb = new StringBuilder();
    //    foreach (var s in split)
    //    {
    //        var attributeType = int.Parse(s);
    //        if (attributeType < 0) continue;
    //        var typeName = AttributeList[attributeType];
    //        sb.Append(typeName);
    //        sb.Append(Environment.NewLine);
    //    }

    //    return sb.ToString();
    //}

    public bool IsValid(int attributeType)
    {
        return AttributeList.ContainsKey(attributeType);
    }
}