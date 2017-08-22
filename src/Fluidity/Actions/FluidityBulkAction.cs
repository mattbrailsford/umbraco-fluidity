namespace Fluidity.Actions
{
    public abstract class FluidityBulkAction
    {
        public abstract string Icon { get; }

        public abstract string Alias { get; }

        public abstract string Name { get; }

        public abstract string AngularServiceName { get; }
    }
}
