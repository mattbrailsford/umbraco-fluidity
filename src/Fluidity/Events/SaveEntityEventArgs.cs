using Fluidity.Models;

namespace Fluidity.Events
{
    public class SavingEntityEventArgs : SavedEntityEventArgs
    {
        public bool Cancel { get; set; }
    }

    public class SavedEntityEventArgs : EntityEventArgs<BeforeAndAfter<object>>
    { }
}
