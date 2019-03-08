// <copyright file="SqlExtensions.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq.Expressions;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Fluidity.Extensions
{
    internal static class SqlExtensions
    {
        public static Sql OrderBy(this Sql sql, Type type, LambdaExpression orderBy, ISqlSyntaxProvider syntaxProvider)
        {
            var method = typeof(PetaPocoSqlExtensions).GetGenericMethod("OrderBy", new[] { type }, new[] { typeof(Sql), typeof(Expression<>), typeof(ISqlSyntaxProvider) });
            var generic = method.MakeGenericMethod(type);
            generic.Invoke(null, new object[] { sql, orderBy, syntaxProvider });
            return sql;
        }

        public static Sql OrderByDescending(this Sql sql, Type type, LambdaExpression orderByDesc, ISqlSyntaxProvider syntaxProvider)
        {
            var method = typeof(PetaPocoSqlExtensions).GetGenericMethod("OrderByDescending", new[] { type }, new[] { typeof(Sql), typeof(Expression<>), typeof(ISqlSyntaxProvider) });
            var generic = method.MakeGenericMethod(type);
            generic.Invoke(null, new object[] { sql, orderByDesc, syntaxProvider });
            return sql;
        }

        public static Sql Where(this Sql sql, Type type, LambdaExpression whereClause, ISqlSyntaxProvider syntaxProvider)
        {
            var method = typeof(PetaPocoSqlExtensions).GetGenericMethod("Where", new[] { type }, new[] { typeof(Sql), typeof(Expression<>), typeof(ISqlSyntaxProvider) });
            var generic = method.MakeGenericMethod(type);
            generic.Invoke(null, new object[] { sql, whereClause, syntaxProvider });
            return sql;
        }
    }
}
