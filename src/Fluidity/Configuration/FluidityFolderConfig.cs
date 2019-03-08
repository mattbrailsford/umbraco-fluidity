// <copyright file="FluidityFolderConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity folder configuration
    /// </summary>
    /// <seealso cref="Fluidity.Configuration.FluidityContainerTreeItemConfig" />
    public class FluidityFolderConfig : FluidityContainerTreeItemConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityFolderConfig"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="config">A configuration delegate.</param>
        public FluidityFolderConfig(string name, string icon = null, Action<FluidityFolderConfig> config = null)
            : base(name, icon)
        {
            config?.Invoke(this);
        }

        /// <summary>
        /// Changes the alias of the folder.
        /// </summary>
        /// <remarks>
        /// An alias will automatically be generated from the folder name however you can use SetAlias to change this if required
        /// </remarks>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public FluidityFolderConfig SetAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        /// <summary>
        /// Changes the icon color of the folder icon.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public FluidityFolderConfig SetIconColor(string color)
        {
            _iconColor = color;
            return this;
        }
    }
}