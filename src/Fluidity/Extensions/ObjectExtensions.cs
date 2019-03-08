// <copyright file="ObjectExtensions.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Fluidity.Configuration;

namespace Fluidity.Extensions
{
    internal static class ObjectExtensions
    {
        public static object GetPropertyValue(this object instance, FluidityPropertyConfig config)
        {
            if (config.PropertyGetter != null)
            {
                return config.PropertyGetter.Invoke(instance);
            }
            else
            {
                return GetPropertyValue(instance, config.PropertyInfo.Name);
            }
        }

        public static object GetPropertyValue(this object instance, string propertyName)
        {
            return instance.GetType().GetPropertyValue(propertyName, instance);
        }

        public static void SetPropertyValue(this object instance, FluidityPropertyConfig config, object value)
        {
            if (config.PropertySetter != null)
            {
                config.PropertySetter.Invoke(instance, value);
            }
            else
            {
                SetPropertyValue(instance, config.PropertyInfo.Name, value);
            }
        }

        public static void SetPropertyValue(this object instance, string propertyName, object value)
        {
            var prop = instance.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop.CanWrite)
            {
                prop.SetValue(instance, value);
            }
        }

        public static Guid EncodeAsGuid(this object obj)
        {
            Guid result;
            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.Default.GetBytes(obj.ToString()));
                Array.Resize(ref hash, 16); // Needs to be 128 bits max
                result = new Guid(hash);
            }
            return result;
        }
    }
}
