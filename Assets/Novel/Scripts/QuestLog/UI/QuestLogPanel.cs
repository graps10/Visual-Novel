using System;
using System.Collections.Generic;
using Naninovel;
using Naninovel.UI;
using UnityEngine;

public class QuestLogPanel : CustomUI, IQuestLogUI, ILocalizableUI
{
    [Serializable]
    public new class GameState
    {
        public List<Quest> ActiveQuests;
        public List<Quest> CompletedQuests;
    }

    [SerializeField] private RectTransform activeQuestsContainer;
    [SerializeField] private RectTransform completedQuestsContainer;
    [SerializeField] private QuestLogMessageUI questEntryPrefab;
    [SerializeField] private QuestLogMessageUI questUpdatePrefab;

    private readonly Dictionary<string, QuestLogMessageUI> questEntries = new Dictionary<string, QuestLogMessageUI>();
    private readonly Stack<QuestLogMessageUI> entriesPool = new Stack<QuestLogMessageUI>();
    private IQuestLogManager questManager;
    private IInputManager inputManager;

    public virtual void AddMessage(LocalizableText text) => SpawnMessage(new QuestLogMessage(text));

    public virtual void AppendMessage(LocalizableText text)
    {
        if (questEntries.Count > 0)
        {
            var lastEntry = questEntries.Values.GetEnumerator().Current;
            lastEntry?.Append(text);
        }
    }

    public virtual void Clear()
    {
        foreach (var entry in questEntries.Values)
        {
            entry.gameObject.SetActive(false);
            entriesPool.Push(entry);
        }
        questEntries.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
        inputManager = Engine.GetService<IInputManager>();
        questManager = Engine.GetService<IQuestLogManager>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        questManager.OnQuestUpdated += UpdateQuestDisplay;
        questManager.OnQuestCompleted += CompleteQuestDisplay;

        if (inputManager.TryGetSampler(InputNames.ShowQuestLog, out var show))
            show.OnStart += Show;
        if (inputManager.TryGetSampler(InputNames.Cancel, out var cancel))
            cancel.OnEnd += Hide;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        questManager.OnQuestUpdated -= UpdateQuestDisplay;
        questManager.OnQuestCompleted -= CompleteQuestDisplay;

        if (inputManager.TryGetSampler(InputNames.ShowQuestLog, out var show))
            show.OnStart -= Show;
        if (inputManager.TryGetSampler(InputNames.Cancel, out var cancel))
            cancel.OnEnd -= Hide;
    }

    private void UpdateQuestDisplay(Quest quest)
    {
        if (!questEntries.TryGetValue(quest.Id, out var entry))
        {
            entry = Instantiate(questEntryPrefab, activeQuestsContainer);
            entry.Initialize(new QuestLogMessage(quest.Title));
            questEntries.Add(quest.Id, entry);
        }

        if (quest.Updates.Count > 0)
        {
            var updateEntry = Instantiate(questUpdatePrefab, entry.transform);
            updateEntry.Initialize(new QuestLogMessage(quest.Updates[^1]));
        }
    }

    private void CompleteQuestDisplay(string questId)
    {
        if (questEntries.TryGetValue(questId, out var entry))
        {
            // entry.gameObject.SetActive(false);
            // entry.transform.SetParent(completedQuestsContainer, false);
            Clear();
        }
    }

    private QuestLogMessageUI GetOrCreateEntry()
    {
        if (entriesPool.Count > 0) return entriesPool.Pop();
        return Instantiate(questEntryPrefab);
    }

    protected virtual void SpawnMessage(QuestLogMessage message)
    {
        var entry = GetOrCreateEntry();
        entry.Initialize(message);
        entry.transform.SetParent(activeQuestsContainer, false);
    }
}
