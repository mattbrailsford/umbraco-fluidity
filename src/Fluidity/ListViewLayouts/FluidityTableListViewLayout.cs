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
