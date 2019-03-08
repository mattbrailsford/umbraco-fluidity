// <copyright file="FluidityEditorConfig`T.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity editor configuration
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity.</typeparam>
    /// <seealso cref="Fluidity.Configuration.FluidityEditorConfig" />
    public class FluidityEditorConfig<TEntityType> : FluidityEditorConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityEditorConfig{TEntityType}"/> class.
        /// </summary>
        /// <param name="config">A configuration delegate.</param>
        public FluidityEditorConfig(Action<FluidityEditorConfig<TEntityType>> config = null)
        {
            config?.Invoke(this);
        }

        /// <summary>
        /// Adds a tab to the editor.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="tabConfig">A tab configuration delegate.</param>
        /// <returns>The tab configuration.</returns>
        public FluidityTabConfig<TEntityType> AddTab(string name, Action<FluidityTabConfig<TEntityType>> tabConfig = null)
        {
            return AddTab(new FluidityTabConfig<TEntityType>(name, tabConfig));
        }

        /// <summary>
        /// Adds a tab to the editor.
        /// </summary>
        /// <param name="tabConfig">The tab configuration.</param>
        /// <returns>The tab configuration.</returns>
        public FluidityTabConfig<TEntityType> AddTab(FluidityTabConfig<TEntityType> tabConfig)
        {
            var tab = tabConfig;
            _tabs.Add(tab);
            return tab;
        }
    }
}