using JetBrains.Application.BuildScript.Application.Zones;

namespace ReSharperPlugin.GuidGenerator
{
    [ZoneDefinition]
    // [ZoneDefinitionConfigurableFeature("Title", "Description", IsInProductSection: false)]
    public interface IGuidGeneratorZone : IZone
    {
    }
}
