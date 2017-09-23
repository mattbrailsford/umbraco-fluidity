// <copyright file="Fluidity.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq;
using Fluidity.Events;
using Fluidity.Data;
using Fluidity.Configuration;

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

        public static FluidityRepositoryProxy<TEntity, object> GetRepository<TEntity>(string sectionAlias = null)
        {
            return GetRepository<TEntity, object>(sectionAlias);
        }
        public static FluidityRepositoryProxy<TEntity, TId> GetRepository<TEntity, TId>(string sectionAlias = null)
        {
            FluidityCollectionConfig collectionConfig;
            if (sectionAlias != null)
            {
                collectionConfig = FluidityContext.Current.Config.Sections[sectionAlias].Tree.FlattenedTreeItems.Values.OfType<FluidityCollectionConfig>().FirstOrDefault(x => x != null && x.EntityType == typeof(TEntity));
            }
            else
            {
                collectionConfig = FluidityContext.Current.Config.Sections.Values.SelectMany(x => x.Tree.FlattenedTreeItems.Values.OfType<FluidityCollectionConfig>()).FirstOrDefault(x => x != null && x.EntityType == typeof(TEntity));
            }

            if (collectionConfig == null)
                throw new ApplicationException($"No collection found for type {typeof(TEntity)}");

            var repo = FluidityContext.Current.Data.RepositoryFactory.GetRepository(collectionConfig);
            return new FluidityRepositoryProxy<TEntity, TId>(repo);
        }
    }
}
