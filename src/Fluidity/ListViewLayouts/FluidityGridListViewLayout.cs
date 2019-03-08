// <copyright file="FluidityGridListViewLayout.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Fluidity.ListViewLayouts
{
    public class FluidityGridListViewLayout : FluidityListViewLayout
    {
        public override string Icon => "icon-thumbnails-small";

        public override string Name => "Grid";

        public override string View => "views/propertyeditors/listview/layouts/grid/grid.html";

        public FluidityGridListViewLayout()
        {
            IsSystem = true;
        }
    }
}
