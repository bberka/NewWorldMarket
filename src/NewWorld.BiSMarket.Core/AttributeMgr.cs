using System.Text;
using NewWorld.BiSMarket.Core.Constants;

namespace NewWorld.BiSMarket.Core;

public class AttributeMgr
{

    private AttributeMgr()
    {
        AttributeList = Enum.GetValues(typeof(AttributeType))
            .Cast<AttributeType>()
            .ToDictionary(t => (int)t, t => t.ToString());
        var  attributeShortList = new Dictionary<string, string>();
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
            Instance ??= new();
            return Instance;
        }
    }
    private static AttributeMgr? Instance;
    public IReadOnlyDictionary<int, string> AttributeList;
    public IReadOnlyDictionary<string, string> AttributeShortList;
    public string ParseAttributeFormattedText(string attributeText)
    {
        var split = attributeText.Split(",");
       
        var sb = new StringBuilder();
        foreach (var s in split)
        {
            var split2 = s.Split(":");
            if (split2.Length == 0)
            {
                continue;
            }

            var attributeType = int.Parse(split2[0]);
            var attributeValue = int.Parse(split2[1]);
            var typeAsShortName = AttributeShortList[AttributeList[attributeType]];
            sb.Append($"+{attributeValue} {typeAsShortName}");
            sb.Append("<br/>");
        }
        return sb.ToString();
    }
}