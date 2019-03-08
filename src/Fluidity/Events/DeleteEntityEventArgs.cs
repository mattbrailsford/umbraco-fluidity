// <copyright file="DeleteEntityEventArgs.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Fluidity.Events
{
    public class DeletingEntityEventArgs : DeletedEntityEventArgs
    {
        public bool Cancel { get; set; }
    }

    public class DeletedEntityEventArgs : EntityEventArgs<object>
    { }
}
