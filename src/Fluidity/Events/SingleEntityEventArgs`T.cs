using System;

namespace Fluidity.Events
{
    public class SingleEntityEventArgs<TEntityType> : EventArgs
    {
        public TEntityType Entity { get; set; }
    }
}
