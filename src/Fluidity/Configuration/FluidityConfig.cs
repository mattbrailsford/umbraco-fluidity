using System;
using System.Collections.Generic;
using Fluidity.Collections;

namespace Fluidity.Configuration
{
    public class FluidityConfig
    {
        protected ConcurrentDictionary<string, FluiditySectionConfig> _sections;
        internal IReadOnlyDictionary<string, FluiditySectionConfig> Sections => _sections;

        public FluidityConfig()
        {
            _sections = new ConcurrentDictionary<string, FluiditySectionConfig>();
        }

        public FluiditySectionConfig AddSection(string name, Action<FluiditySectionConfig> sectionConfig = null)
        {
            return AddSection(new FluiditySectionConfig(name, config : sectionConfig));
        }

        public FluiditySectionConfig AddSection(string name, string icon, Action<FluiditySectionConfig> sectionConfig = null)
        {
            return AddSection(new FluiditySectionConfig(name, icon, sectionConfig));
        }

        public FluiditySectionConfig AddSection(FluiditySectionConfig sectionConfig)
        {
            var section = sectionConfig;
            _sections.AddOrUpdate(section.Alias, section, (s, config) => config);
            return section;
        }

        internal void PostProcess()
        {
            foreach (var section in _sections.Values)
            {
                section.PostProcess();
            }
        }
    }
}
