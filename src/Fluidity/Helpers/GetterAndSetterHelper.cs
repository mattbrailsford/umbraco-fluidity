using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fluidity.Extensions;
using Fluidity.Models;

namespace Fluidity.Helpers
{
    internal static class GetterAndSetterHelper
    {
        internal static GetterAndSetter<TEntityType, TValueType> Create<TEntityType, TValueType>(Expression<Func<TEntityType, TValueType>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression != null)
            {
                var name = "";
                var propertyInfo = propertyExpression.GetPropertyInfo();
                var setMethod = propertyInfo.GetSetMethod();

                var parameterT = Expression.Parameter(typeof(TEntityType), "x");
                var parameterTProperty = Expression.Parameter(typeof(TValueType), "y");
                Expression instanceExpression;
                var declaringType = setMethod.DeclaringType;

                if (declaringType == typeof(TEntityType))
                {
                    instanceExpression = parameterT;
                    name = propertyInfo.Name;
                }
                else
                {
                    var objectGetters = new List<MethodInfo>();
                    var objectNames = new List<string>
                    {
                        propertyInfo.Name
                    };

                    var getObjectExpression = memberExpression.Expression as MemberExpression;

                    // traverse the nested object towards the TEntityType
                    while (declaringType != typeof(TEntityType))
                    {
                        var objectGetterPropertyInfo = getObjectExpression.Member as PropertyInfo;
                        var objectGetMethod = objectGetterPropertyInfo.GetGetMethod();

                        objectGetters.Add(objectGetMethod);
                        objectNames.Add(objectGetterPropertyInfo.Name);

                        declaringType = objectGetMethod.DeclaringType;
                        getObjectExpression = getObjectExpression.Expression as MemberExpression;
                    }

                    // if the property expression is x => x.A.B.C.Property
                    // the objectGetters order we get is: get_C, get_B, get_A
                    // so reversing it allows for building the set expression:
                    // (x, y) => x.get_A.get_B.get_C.Property = y
                    objectGetters.Reverse();
                    objectNames.Reverse();

                    var objectGetterExpression = objectGetters.Aggregate(
                        parameterT as Expression,
                        (parameter, method) => Expression.Call(parameter, method));

                    instanceExpression = objectGetterExpression;

                    name = string.Join("", objectNames);
                }

                var setExpression =
                    Expression.Lambda<Action<TEntityType, TValueType>>(
                        Expression.Call(instanceExpression, setMethod, parameterTProperty),
                        parameterT,
                        parameterTProperty
                    );

                return new GetterAndSetter<TEntityType, TValueType>
                {
                    PropertyName = name,

                    Getter = propertyExpression.Compile(),
                    Setter = setExpression.Compile()
                };
            }
            else
            {
                return null;
            }
        }
    }
}
