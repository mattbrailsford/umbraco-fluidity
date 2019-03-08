// <copyright file="TreeNodeExtensions.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

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
