using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogEvents : MonoBehaviour
{
    public static QuestLogEvents Instance { get; private set; }

    public event Action<Quest> OnQuestUpdated;
    public event Action<string> OnQuestCompleted;

    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartQuest(string questId, string title)
    {
        var quest = new Quest(questId, title);
        activeQuests[questId] = quest;
        OnQuestUpdated?.Invoke(quest);
    }

    public void UpdateQuest(string questId, string updateText)
    {
        if (activeQuests.TryGetValue(questId, out var quest))
        {
            quest.AddUpdate(updateText);
            OnQuestUpdated?.Invoke(quest);
        }
    }

    public void CompleteQuest(string questId)
    {
        if (activeQuests.Remove(questId, out var quest))
        {
            OnQuestCompleted?.Invoke(questId);
        }
    }
}