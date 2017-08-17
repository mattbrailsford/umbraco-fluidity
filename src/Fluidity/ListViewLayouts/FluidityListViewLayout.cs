namespace Fluidity.ListViewLayouts
{
    public abstract class FluidityListViewLayout
    {
        public abstract string Icon { get; }

        public abstract string Name { get; }

        public abstract string View { get; }

        internal bool IsSystem { get; set; }
    }
}
