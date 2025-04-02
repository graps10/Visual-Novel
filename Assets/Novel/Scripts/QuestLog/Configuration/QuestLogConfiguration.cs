using Naninovel;
using UnityEngine;

[EditInProjectSettings]
public class QuestLogConfiguration : Configuration
{
    public const string DefaultPathPrefix = "QuestLogManager";
    [Tooltip("Configuration of the resource loader used with quest log resources.")]
    public ResourceLoaderConfiguration Loader = new ResourceLoaderConfiguration { PathPrefix = DefaultPathPrefix };
}