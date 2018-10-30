// <copyright file="IFluidityRepository.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Fluidity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Umbraco.Core.Models;

namespace Fluidity.Data
{
    [Obsolete("Use the abstract class FluidityRepository<TEntity, TId> instead")]
    public interface IFluidityRepository : IDisposable
    {
        Type EntityType { get; }

        Type IdType { get; }

        object Get(object id, bool fireEvents = true);

        IEnumerable<object> GetAll(bool fireEvents = true);

        PagedResult<object> GetPaged(int pageNumber, int pageSize, LambdaExpression whereClause, LambdaExpression orderBy, SortDirection orderDirection, bool fireEvents = true);

        object Save(object entity, bool fireEvents = true);

        void Delete(object id, bool fireEvents = true);

        long GetTotalRecordCount(bool fireEvents = true);

        LambdaExpression CreateQueryExpression(FluidityCollectionConfig collection, ParameterExpression parameter, string query);
    }
}
