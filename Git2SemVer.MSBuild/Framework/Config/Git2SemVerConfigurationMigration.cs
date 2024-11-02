using System.Text.Json.Serialization.Metadata;


namespace NoeticTools.Git2SemVer.MSBuild.Framework.Config;

internal static class Git2SemVerConfigurationMigration
{
    public static void Version0JsonModifier(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        foreach (var property in typeInfo.Properties)
        {
            if (property.Name.Equals("Version"))
            {
                //property.CustomConverter = new .ShouldSerialize = (x, y) => false;
            }
        }
    }
}