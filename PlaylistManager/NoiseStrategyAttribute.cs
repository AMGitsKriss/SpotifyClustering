using PlaylistManager.Strategies;
using PlaylistManager.Strategies.NoiseResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PlaylistManager
{
    public class NoiseStrategyAttribute : Attribute
    {
        public NoiseStrategy Strategy { get; set; }

        public NoiseStrategyAttribute(NoiseStrategy strategy)
        {
            Strategy = strategy;
        }

        public static INoiseResolutionStrategy GetInstance(NoiseStrategy strategy)
        {
            var targetType = Assembly.GetExecutingAssembly()
                                     .GetTypes()
                                     .Where(t => typeof(INoiseResolutionStrategy).IsAssignableFrom(t) && t.GetCustomAttribute<NoiseStrategyAttribute>()?.Strategy == strategy)
                                     .FirstOrDefault();

            if (targetType != null)
                return Activator.CreateInstance(targetType) as INoiseResolutionStrategy;

            return null;
        }

    }
}
