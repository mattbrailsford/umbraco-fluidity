using System;

namespace Fluidity.Events
{
    public class EntityEventArgs<TEntityType> : EventArgs
    {
        public TEntityType Entity { get; set; }
    }
}
