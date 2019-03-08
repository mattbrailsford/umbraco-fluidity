// <copyright file="SaveEntityEventArgs.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Fluidity.Models;

namespace Fluidity.Events
{
    public class SavingEntityEventArgs : SavedEntityEventArgs
    {
        public bool Cancel { get; set; }
    }

    public class SavedEntityEventArgs : EntityEventArgs<BeforeAndAfter<object>>
    { }
}
