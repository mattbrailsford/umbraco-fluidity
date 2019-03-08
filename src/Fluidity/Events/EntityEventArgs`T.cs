// <copyright file="EntityEventArgs`T.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;

namespace Fluidity.Events
{
    public class EntityEventArgs<TEntityType> : EventArgs
    {
        public TEntityType Entity { get; set; }
    }
}
