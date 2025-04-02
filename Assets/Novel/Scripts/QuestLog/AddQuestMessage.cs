using Naninovel;

[CommandAlias("addQuestMessage")]
public class AddQuestMessage : Command
{
    [ParameterAlias("text")]
    public StringParameter QuestText;

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        if (QuestLogEvents.Instance != null && QuestText.HasValue)
        {
            LocalizableText localizedText = LocalizableText.FromPlainText(QuestText);
            QuestLogEvents.Instance.AddQuestMessage(localizedText);
        }
        return UniTask.CompletedTask;
    }
}

