// <copyright file="TypeExtensions.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Fluidity.Extensions
{
    internal static class TypeExtensions
    {
        public static string GetTableName(this Type type)
        {
            var attr = type.GetCustomAttribute<TableNameAttribute>(false);
            var name = attr?.Value.Trim('[', ']') ?? type.Name;
            return name;
        }

        public static string GetPrimaryKeyColumnName(this Type type, string defaultValue = "Id")
        {
            var attr = type.GetCustomAttribute<PrimaryKeyAttribute>(true);
            if (attr != null)
                return attr.Value.Trim('[', ']');

            var attr2 = type.FindPrimaryKeyColumn();
            if (attr2 != null)
                return attr2.Name.Trim('[', ']');

            return defaultValue;
        }

        public static bool AutoIncrementPrimaryKey(this Type type)
        {
            var attr = type.GetCustomAttribute<PrimaryKeyAttribute>(true);
            if (attr != null)
                return attr.autoIncrement;

            var attr2 = type.FindPrimaryKeyColumn();
            return attr2 != null && attr2.AutoIncrement;
        }

        private static PrimaryKeyColumnAttribute FindPrimaryKeyColumn(this Type type)
        {
            foreach (var propertyInfo in type.GetProperties())
            {
                var attr = propertyInfo.GetCustomAttribute<PrimaryKeyColumnAttribute>();
                if (attr != null)
                {
                    if (string.IsNullOrWhiteSpace(attr.Name))
                    {
                        attr.Name = propertyInfo.Name;
                    }
                    return attr;
                }
            }
            return null;
        }

        public static object GetPropertyValue(this Type type, string propertyName, object instance)
        {
            var propertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            return propertyInfo?.GetValue(instance, null);
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, Type[] genericArgTypes, Type[] paramTypes)
        {
            return type
                .GetMethods()
                .Where(m => m.Name == name)
                .Select(m => new {
                    Method = m,
                    ParamTypes = m.GetParameters().Select(x => x.ParameterType).ToArray(),
                    GenericArgTypes = m.GetGenericArguments()
                })
                .Where(x => x.ParamTypes.Length == paramTypes.Length && x.ParamTypes.SequenceEqual(paramTypes, new SimpleTypeComparer())
                    && x.GenericArgTypes.Length == genericArgTypes.Length)
                .Select(x => x.Method)
                .FirstOrDefault();
        }

        private class SimpleTypeComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                return x.Assembly == y.Assembly &&
                    x.Namespace == y.Namespace &&
                    x.Name == y.Name;
            }

            public int GetHashCode(Type obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
