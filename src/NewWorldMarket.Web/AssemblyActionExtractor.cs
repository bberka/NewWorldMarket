using System.Reflection;
using System.Text;
using EasMe.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NewWorldMarket.Web;

#if DEBUG
public static class AssemblyActionExtractor
{
    public static void ExtractEnumFromControllerActions()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "extracted.cs");
        var assembly = Assembly.GetExecutingAssembly();
        var controllerTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(ControllerBase))).ToList();
        var list = new List<string>();
        foreach (var controller in controllerTypes)
        {
            var actions = controller.GetMethods().Where(x => x.IsPublic && x.DeclaringType == controller).ToList();
            foreach (var action in actions)
            {
                var actionName = action.Name;
                var route = controller.Name + "/" + actionName;
                var enumStr = controller.Name + "_" + actionName;
                list.Add(enumStr + "," + " //" + route);
            }
        }
        var sb = new StringBuilder();
        sb.AppendLine("namespace NAMESPACE_PLACEHOLDER;");
        sb.AppendLine("");
        sb.AppendLine("public enum ActionType");
        sb.AppendLine("{");
        foreach (var action in list.Distinct())
        {

            sb.AppendLine("     "+ action.RemoveText("Controller"));
            
        }
        sb.AppendLine("}");
        File.WriteAllText(path, sb.ToString());
    }
}
#endif