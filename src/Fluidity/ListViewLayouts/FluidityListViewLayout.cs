// <copyright file="FluidityListViewLayout.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Fluidity.ListViewLayouts
{
    public abstract class FluidityListViewLayout
    {
        public abstract string Icon { get; }

        public abstract string Name { get; }

        public abstract string View { get; }

        internal bool IsSystem { get; set; }
    }
}
