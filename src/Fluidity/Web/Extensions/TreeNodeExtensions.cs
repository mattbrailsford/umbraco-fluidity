using Umbraco.Core;
using Umbraco.Web.Models.Trees;

namespace Fluidity.Web.Extensions
{
    internal static class TreeNodeExtensions
    {
        internal static void SetColorStyle(this TreeNode treeNode, string color)
        {
            // Remove any existing color classes
            treeNode.CssClasses.RemoveAll(x => x.StartsWith("color-"));

            // Add the color class
            treeNode.CssClasses.Add("color-" + color);
        }
    }
}
