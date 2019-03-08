// <copyright file="Fluidity.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq;
using Umbraco.Core;
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

        public static FluidityRepository<TEntity, TId> GetRepository<TEntity, TId>()
        {
            return GetRepository<TEntity, TId>(null, null);
        }

        public static FluidityRepository<TEntity, TId> GetRepository<TEntity, TId>(string sectionAlias)
        {
            return GetRepository<TEntity, TId>(sectionAlias, null);
        }

        public static FluidityRepository<TEntity, TId> GetRepository<TEntity, TId>(string sectionAlias, string collectionAlias)
        {
            // Get the relevant collection config
            var config = FluidityContext.Current.Config;
            var collections = !sectionAlias.IsNullOrWhiteSpace()
                ? config.Sections[sectionAlias].Tree.FlattenedTreeItems.Values.OfType<FluidityCollectionConfig>()
                : config.Sections.Values.SelectMany(x => x.Tree.FlattenedTreeItems.Values.OfType<FluidityCollectionConfig>());

            var collectionConfig = collections.FirstOrDefault(x => (collectionAlias.IsNullOrWhiteSpace() || x.Alias == collectionAlias) && x.EntityType == typeof(TEntity));

            if (collectionConfig == null)
                throw new ApplicationException($"No collection found for type {typeof(TEntity)}");

            // Get the registred repository for collection
            var repo = FluidityContext.Current.Data.RepositoryFactory.GetRepository(collectionConfig);

            // See if someone already registered a custom repo of this type
            var typedRepo = repo as FluidityRepository<TEntity, TId>;
            if (typedRepo != null)
                return typedRepo;

            // See if it's a default repo implementation and if so proxy it
            var defaultRepo = repo as DefaultFluidityRepository;
            if (defaultRepo != null)
                return new DefaultFluidityRepositoryProxy<TEntity, TId>(defaultRepo);

            // Unknown repository type
            throw new ApplicationException($"Unknown repository type. Cannot convert repository of type {repo.GetType()} to {typeof(FluidityRepository<TEntity, TId>)}");
        }
    }
}
