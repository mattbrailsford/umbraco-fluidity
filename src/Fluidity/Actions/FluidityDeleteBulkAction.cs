// <copyright file="FluidityDeleteBulkAction.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Fluidity.Actions
{
    public class FluidityDeleteBulkAction : FluidityBulkAction
    {
        public override string Icon => "icon-trash";

        public override string Alias => "delete";

        public override string Name => "Delete";

        public override string AngularServiceName => "fluidityDeleteBulkActionService";
    }
}
