using Naninovel;
using UnityEngine;

[InitializeAtRuntime]
public class QuestLogManager : IEngineService
{
    public virtual QuestLogConfiguration Configuration { get; }
    private GameObject questLogObject;

    public QuestLogManager(QuestLogConfiguration config)
    {
        Configuration = config;
    }

    public virtual UniTask InitializeServiceAsync()
    {
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