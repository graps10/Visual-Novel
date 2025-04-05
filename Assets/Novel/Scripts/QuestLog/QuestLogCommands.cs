using Naninovel;

[CommandAlias("addQuest")]
public class AddQuestCommand : Command
{
    [ParameterAlias("title")] public StringParameter Title;
    [ParameterAlias("id")] public StringParameter QuestId;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var questManager = Engine.GetService<IQuestLogManager>();
        await questManager.StartQuestAsync(QuestId, Title, asyncToken);
    }
}

[CommandAlias("addQuestUpdate")]
public class UpdateQuestCommand : Command
{
    [ParameterAlias("id")] public StringParameter QuestId;
    [ParameterAlias("text")] public StringParameter Text;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var questManager = Engine.GetService<IQuestLogManager>();
        await questManager.UpdateQuestAsync(QuestId, Text, asyncToken);
    }
}

[CommandAlias("completeQuest")]
public class CompleteQuestCommand : Command
{
    [ParameterAlias("id")] public StringParameter QuestId;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var questManager = Engine.GetService<IQuestLogManager>();
        await questManager.CompleteQuestAsync(QuestId, asyncToken);
    }
}

