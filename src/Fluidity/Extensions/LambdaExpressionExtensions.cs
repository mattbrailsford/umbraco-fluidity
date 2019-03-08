// <copyright file="LambdaExpressionExtensions.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluidity.Extensions
{
    internal static class LambdaExpressionExtensions
    {
        public static PropertyInfo GetPropertyInfo(this LambdaExpression propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                var unary = propertyLambda.Body as UnaryExpression;
                if (unary != null)
                {
                    member = unary.Operand as MemberExpression;
                }
            }

            if (member == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            //var type = typeof(TSource);
            //if (propInfo.ReflectedType != null && type != propInfo.ReflectedType
            //    && !type.IsSubclassOf(propInfo.ReflectedType))
            //{
            //    throw new ArgumentException($"Expresion '{propertyLambda}' refers to a property that is not from type {type}.");
            //}

            return propInfo;
        }
    }
}
