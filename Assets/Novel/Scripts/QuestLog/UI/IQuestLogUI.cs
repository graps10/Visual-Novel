using Naninovel;
using Naninovel.UI;

public interface IQuestLogUI : IManagedUI
{
    void AddMessage(LocalizableText text);

    void AppendMessage(LocalizableText text);

    void Clear();
}
