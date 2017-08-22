namespace Fluidity.Events
{
    public class DeletingEntityEventArgs : DeletedEntityEventArgs
    {
        public bool Cancel { get; set; }
    }

    public class DeletedEntityEventArgs : EntityEventArgs<object>
    { }
}
