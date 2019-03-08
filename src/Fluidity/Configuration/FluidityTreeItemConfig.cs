// <copyright file="FluidityTreeItemConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Base class for Fluidity tree item configuration
    /// </summary>
    public abstract class FluidityTreeItemConfig
    {
        protected string _alias;
        internal string Alias => _alias;

        protected string _iconColor;
        internal string IconColor => _iconColor;

        protected string _parentAlias;
        internal string ParentAlias => _parentAlias;

        protected string _parentPath;
        internal string ParentPath => _parentAlias;

        protected string _path;
        internal string Path => _path;

        internal int Ordinal { get; set; }

        /// <summary>
        /// Used in post processing to work out tree item paths once config is complete.
        /// </summary>
        /// <param name="parentPath">The parent path.</param>
        internal void UpdatePaths(string parentPath)
        {
            _parentPath = parentPath;

            var seperatorIndex = _parentPath.LastIndexOf(FluidityConstants.PATH_SEPERATOR, StringComparison.InvariantCulture);
            _parentAlias = seperatorIndex >= 0 ? _parentPath.Substring(seperatorIndex + FluidityConstants.PATH_SEPERATOR.Length) : _parentPath;

            _path = _parentPath + FluidityConstants.PATH_SEPERATOR + _alias;
        }
    }
}