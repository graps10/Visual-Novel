using Naninovel;

[CommandAlias("addQuest")]
public class AddQuestCommand : Command
{
    [ParameterAlias("title")] public StringParameter Title;
    [ParameterAlias("id")] public StringParameter QuestId;

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        QuestLogEvents.Instance?.StartQuest(QuestId, Title);
        return UniTask.CompletedTask;
    }
}

[CommandAlias("addQuestUpdate")]
public class UpdateQuestCommand : Command
{
    [ParameterAlias("id")] public StringParameter QuestId;
    [ParameterAlias("text")] public StringParameter Text;

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        QuestLogEvents.Instance?.UpdateQuest(QuestId, Text);
        return UniTask.CompletedTask;
    }
}

[CommandAlias("completeQuest")]
public class CompleteQuestCommand : Command
{
    [ParameterAlias("id")] public StringParameter QuestId;

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        QuestLogEvents.Instance?.CompleteQuest(QuestId);
        return UniTask.CompletedTask;
    }
}

