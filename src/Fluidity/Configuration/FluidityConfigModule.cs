using Umbraco.Core;

namespace Fluidity.Configuration
{
    public abstract class FluidityConfigModule : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            FluidityBootManager.FluidityStarting += (sender, args) => {
                Configure(args.Config);
            };
        }

        public abstract void Configure(FluidityConfig config);
    }
}
