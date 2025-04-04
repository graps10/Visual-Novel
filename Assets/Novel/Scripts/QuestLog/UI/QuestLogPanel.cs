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
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        QuestLogEvents.Instance.OnQuestUpdated += UpdateQuestDisplay;
        QuestLogEvents.Instance.OnQuestCompleted += CompleteQuestDisplay;

        if (inputManager.TryGetSampler(InputNames.ShowQuestLog, out var show))
            show.OnStart += Show;
        if (inputManager.TryGetSampler(InputNames.Cancel, out var cancel))
            cancel.OnEnd += Hide;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        QuestLogEvents.Instance.OnQuestUpdated -= UpdateQuestDisplay;
        QuestLogEvents.Instance.OnQuestCompleted -= CompleteQuestDisplay;

        if (inputManager.TryGetSampler(InputNames.ShowQuestLog, out var show))
            show.OnStart -= Show;
        if (inputManager.TryGetSampler(InputNames.Cancel, out var cancel))
            cancel.OnEnd -= Hide;
    }

    private void UpdateQuestDisplay(Quest quest)
    {
        if (!questEntries.TryGetValue(quest.Id, out var entry))
        {
            entry = GetOrCreateEntry();

            string formattedTitle = quest.Title;
            entry.Initialize(new QuestLogMessage(formattedTitle));

            entry.transform.SetParent(activeQuestsContainer, false);
            questEntries.Add(quest.Id, entry);
        }

        if (quest.Updates.Count == 0) return;

        string latestUpdate = quest.Updates[quest.Updates.Count - 1];

        var updateEntry = Instantiate(questUpdatePrefab, entry.transform);
        updateEntry.Initialize(new QuestLogMessage(latestUpdate));
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
