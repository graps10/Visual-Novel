using System;
using System.Collections.Generic;
using Naninovel;

[InitializeAtRuntime]
public class QuestLogManager : IQuestLogManager
{
    public event Action<Quest> OnQuestUpdated;
    public event Action<string> OnQuestCompleted;
    public event Action OnQuestNotification;

    private readonly Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();
    private readonly IInputManager inputManager;

    public QuestLogManager(IInputManager inputManager)
    {
        this.inputManager = inputManager;
    }

    public UniTask InitializeServiceAsync() => UniTask.CompletedTask;
    public void ResetService() => activeQuests.Clear();
    public void DestroyService() { }

    public bool HasActiveQuest(string questId) => activeQuests.ContainsKey(questId);

    public Quest GetQuest(string questId) => activeQuests.TryGetValue(questId, out var quest) ? quest : null;

    public async UniTask StartQuestAsync(string questId, string title, AsyncToken asyncToken = default)
    {
        var quest = new Quest(questId, title);
        activeQuests[questId] = quest;
        OnQuestUpdated?.Invoke(quest);
        await UniTask.CompletedTask;
    }

    public async UniTask UpdateQuestAsync(string questId, string updateText, AsyncToken asyncToken = default)
    {
        if (activeQuests.TryGetValue(questId, out var quest))
        {
            quest.AddUpdate(updateText);
            OnQuestUpdated?.Invoke(quest);
            OnQuestNotification?.Invoke();
        }
        await UniTask.CompletedTask;
    }

    public async UniTask CompleteQuestAsync(string questId, AsyncToken asyncToken = default)
    {
        if (activeQuests.Remove(questId, out _))
        {
            OnQuestCompleted?.Invoke(questId);
            OnQuestNotification?.Invoke();
        }
        await UniTask.CompletedTask;
    }
}