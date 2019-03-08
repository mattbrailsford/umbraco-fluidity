// <copyright file="BeforeAndAfter`T.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Fluidity.Models
{
    public class BeforeAndAfter<TEntityType>
    {
        public TEntityType Before { get; set; }
        public TEntityType After { get; set; }
    }
}
