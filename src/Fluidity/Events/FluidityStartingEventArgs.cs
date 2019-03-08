// <copyright file="FluidityStartingEventArgs.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using Fluidity.Configuration;

namespace Fluidity.Events
{
    internal class FluidityStartingEventArgs : EventArgs
    {
        public FluidityConfig Config { get; set; }
    }
}