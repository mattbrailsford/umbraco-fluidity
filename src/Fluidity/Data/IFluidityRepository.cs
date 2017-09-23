// <copyright file="IFluidityRepository.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Umbraco.Core.Models;

namespace Fluidity.Data
{
    [Obsolete("Use the abstract class FluidityRepository<TEntity, TId> instead")]
    public interface IFluidityRepository
    {
        Type EntityType { get; }

        object Get(object id);

        IEnumerable<object> GetAll();

        PagedResult<object> GetPaged(int pageNumber, int pageSize, LambdaExpression orderBy, SortDirection orderDirection, LambdaExpression whereClause);

        object Save(object entity);

        void Delete(object id);

        long GetTotalRecordCount();
    }
}