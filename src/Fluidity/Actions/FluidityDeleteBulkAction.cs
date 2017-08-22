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
