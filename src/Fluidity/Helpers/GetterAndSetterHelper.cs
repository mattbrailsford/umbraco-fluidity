// <copyright file="GetterAndSetterHelper.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fluidity.Models;

namespace Fluidity.Helpers
{
    internal static class GetterAndSetterHelper
    {
        /// <summary>
        /// Converts a given LambdaExpression containing MemberExpressions to getters and setters, and the name of each nested object plus the name of the property.
        /// 
        /// (Person x) => x.Company.Owner.Name becomes:
        /// getter: (object x) => (object)(((Person) x).get_Company().get_Owner().get_Name())
        /// setter: (object x, object y) => ((Person) x).get_Company().get_Owner().set_Name((string)y)
        /// name: CompanyOwnerName
        /// </summary>
        /// <param name="lambdaExpression">The LambdaExpression to be converted</param>
        /// <returns>GetterAndSetter object when successful, null when not.</returns>
        internal static GetterAndSetter Create(LambdaExpression lambdaExpression)
        {
            try
            {
                var isAtTail = true;

                var parameterT = Expression.Parameter(typeof(object), "x");
                Type parameterTType = null;
                var parameterTProperty = Expression.Parameter(typeof(object), "y");
                Type parameterTPropertyType = null;

                MethodInfo setValueMethod = null;
                MethodInfo getValueMethod = null;
                var names = new List<string>();
                var getNestedObjectMethods = new List<MethodInfo>();

                var x = lambdaExpression.Body;

                do
                {
                    var memberExpression = x as MemberExpression;
                    var parameterExpression = x as ParameterExpression;

                    if (memberExpression != null)
                    {
                        var propertyInfo = memberExpression.Member as PropertyInfo;
                        if (isAtTail)
                        {
                            setValueMethod = propertyInfo.GetSetMethod();
                            getValueMethod = propertyInfo.GetGetMethod();
                            names.Add(propertyInfo.Name);

                            parameterTPropertyType = propertyInfo.PropertyType;

                            isAtTail = false;

                            x = memberExpression.Expression;
                        }
                        else
                        {
                            getNestedObjectMethods.Insert(0, propertyInfo.GetGetMethod());
                            names.Insert(0, propertyInfo.Name);

                            x = memberExpression.Expression;
                        }
                    }
                    else if (parameterExpression != null)
                    {
                        parameterTType = x.Type;

                        // done, arrived at root
                        break;
                    }
                    else
                    {
                        throw new Exception("Failed to interpret given LambdaExpression");
                    }
                }
                while (true);

                var parameterTAsType = Expression.Convert(parameterT, parameterTType) as Expression;
                var valueAsType = Expression.Convert(parameterTProperty, parameterTPropertyType) as Expression;

                var instanceExpression = (getNestedObjectMethods.Count == 0)
                    ? parameterTAsType
                    : getNestedObjectMethods.Aggregate(
                        parameterTAsType,
                        (parameter, method) => Expression.Call(parameter, method));


                var setExpression =
                    Expression.Lambda<Action<object, object>>(
                        Expression.Call(instanceExpression, setValueMethod, valueAsType),
                        parameterT,
                        parameterTProperty
                    );

                var getExpression =
                    Expression.Lambda<Func<object, object>>(
                        Expression.Call(instanceExpression, getValueMethod),
                        parameterT
                    );

                var name = string.Join("", names);
                var setter = setExpression.Compile();
                var getter = getExpression.Compile();

                return new GetterAndSetter
                {
                    Getter = getter,
                    Setter = setter,
                    PropertyName = name
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
