using System;
using Fluidity.Configuration;

namespace Fluidity.Events
{
    internal class FluidityStartingEventArgs : EventArgs
    {
        public FluidityConfig Config { get; set; }
    }
}