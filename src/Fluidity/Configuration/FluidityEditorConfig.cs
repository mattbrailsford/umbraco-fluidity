// <copyright file="FluidityEditorConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Un typed base class  for a <see cref="FluidityEditorConfig{TEntityType}"/>
    /// </summary>
    public abstract class FluidityEditorConfig
    {
        protected List<FluidityTabConfig> _tabs;
        internal IEnumerable<FluidityTabConfig> Tabs => _tabs;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityEditorConfig"/> class.
        /// </summary>
        protected FluidityEditorConfig()
        {
            _tabs = new List<FluidityTabConfig>();
        }
    }
}