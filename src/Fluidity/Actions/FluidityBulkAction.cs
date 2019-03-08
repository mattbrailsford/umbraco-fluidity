// <copyright file="FluidityBulkAction.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Fluidity.Actions
{
    public abstract class FluidityBulkAction
    {
        public abstract string Icon { get; }

        public abstract string Alias { get; }

        public abstract string Name { get; }

        public abstract string AngularServiceName { get; }
    }
}
