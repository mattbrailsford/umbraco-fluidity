// <copyright file="FluidityEncryptedPropertyConfig.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Linq.Expressions;

namespace Fluidity.Configuration
{
    public class FluidityEncryptedPropertyConfig : FluidityPropertyConfig
    {
        public FluidityEncryptedPropertyConfig(LambdaExpression propertyExp) 
            : base(propertyExp)
        { }
    }
}
