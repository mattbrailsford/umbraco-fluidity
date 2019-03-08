// <copyright file="FluidityTableListViewLayout.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Fluidity.ListViewLayouts
{
    public class FluidityTableListViewLayout : FluidityListViewLayout
    {
        public override string Icon => "icon-list";

        public override string Name => "List";

        public override string View => "views/propertyeditors/listview/layouts/list/list.html";

        public FluidityTableListViewLayout()
        {
            IsSystem = true;
        }
    }
}
