using Naninovel;
using Naninovel.UI;

public interface IQuestLogUI : IManagedUI
{
    /// <param name="text">Text of the message.</param>
    void AddMessage(LocalizableText text);
    /// <summary>
    /// Appends text to the last message of the log (if exists).
    /// </summary>
    /// <param name="text">Text to append to the last message.</param>
    void AppendMessage(LocalizableText text);

    /// <summary>
    /// Removes all messages from the quest log.
    /// </summary>
    void Clear();
}
