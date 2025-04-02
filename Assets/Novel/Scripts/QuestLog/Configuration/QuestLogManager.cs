using Naninovel;
using UnityEngine;

[InitializeAtRuntime]
public class QuestLogManager : IEngineService
{
    public virtual QuestLogConfiguration Configuration { get; }
    private readonly IResourceProviderManager providersManager;
    private ResourceLoader<GameObject> loader;
    private GameObject questLogObject;

    public QuestLogManager(QuestLogConfiguration config, IResourceProviderManager providersManager)
    {
        Configuration = config;
        this.providersManager = providersManager;
    }

    public virtual UniTask InitializeServiceAsync()
    {
        loader = Configuration.Loader.CreateFor<GameObject>(providersManager);
        questLogObject = Engine.CreateObject("QuestLog");
        questLogObject.AddComponent<QuestLogEvents>();
        return UniTask.CompletedTask;
    }

    public virtual void ResetService() { }
    public virtual void DestroyService()
    {
        if (questLogObject) ObjectUtils.DestroyOrImmediate(questLogObject);
    }
}