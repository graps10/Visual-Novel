using System;
using Naninovel;

public interface IQuestLogManager : IEngineService
{
    event Action<Quest> OnQuestUpdated;
    event Action<string> OnQuestCompleted;
    event Action OnQuestNotification;

    bool HasActiveQuest(string questId);
    Quest GetQuest(string questId);
    UniTask StartQuestAsync(string questId, string title, AsyncToken asyncToken = default);
    UniTask UpdateQuestAsync(string questId, string updateText, AsyncToken asyncToken = default);
    UniTask CompleteQuestAsync(string questId, AsyncToken asyncToken = default);
}
