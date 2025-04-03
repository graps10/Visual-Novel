using Naninovel;
using UnityEngine;

[EditInProjectSettings]
public class MiniGameConfiguration : Configuration
{
    public const string DefaultPathPrefix = "MiniGameManager";
    [Tooltip("Configuration of the resource loader used with quest log resources.")]
    public ResourceLoaderConfiguration Loader = new ResourceLoaderConfiguration { PathPrefix = DefaultPathPrefix };
}