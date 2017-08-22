using System;
using Fluidity.Events;

namespace Fluidity
{
    public class Fluidity
    {
        public static event EventHandler<SavingEntityEventArgs> SavingEntity;
        public static event EventHandler<SavedEntityEventArgs> SavedEntity;

        public static event EventHandler<DeletingEntityEventArgs> DeletingEntity;
        public static event EventHandler<DeletedEntityEventArgs> DeletedEntity;

        public static void OnSavingEntity(SavingEntityEventArgs args)
        {
            SavingEntity?.Invoke(null, args);
        }

        public static void OnSavedEntity(SavedEntityEventArgs args)
        {
            SavedEntity?.Invoke(null, args);
        }

        public static void OnDeletingEntity(DeletingEntityEventArgs args)
        {
            DeletingEntity?.Invoke(null, args);
        }

        public static void OnDeletedEntity(DeletedEntityEventArgs args)
        {
            DeletedEntity?.Invoke(null, args);
        }
    }
}
