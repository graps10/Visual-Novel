using System;
using System.Collections.Generic;
using System.Linq;
using Naninovel;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogPanel : CustomUI, IQuestLogUI, ILocalizableUI
{
    [Serializable]
    public new class GameState
    {
        public List<QuestLogMessage> Messages;
    }

    protected virtual QuestLogMessageUI LastMessage => messages.Last?.Value;
    protected virtual RectTransform MessagesContainer => messagesContainer;
    protected virtual ScrollRect ScrollRect => scrollRect;
    protected virtual QuestLogMessageUI MessagePrefab => messagePrefab;
    protected virtual int Capacity => capacity;
    protected virtual int SaveCapacity => saveCapacity;

    [SerializeField] private RectTransform messagesContainer;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private QuestLogMessageUI messagePrefab;
    [Tooltip("How many messages should the backlog keep.")]
    [SerializeField] private int capacity = 300;
    [Tooltip("How many messages should the backlog keep when saving the game.")]
    [SerializeField] private int saveCapacity = 30;
    [Tooltip("Whether to add choices summary to the log.")]

    private readonly LinkedList<QuestLogMessageUI> messages = new LinkedList<QuestLogMessageUI>();
    private readonly Stack<QuestLogMessageUI> messagesPool = new Stack<QuestLogMessageUI>();
    private readonly List<LocalizableText> formatPool = new List<LocalizableText>();
    private IInputManager inputManager;

    public virtual void AddMessage(LocalizableText text)
    {
        SpawnMessage(new QuestLogMessage(text));
    }

    public virtual void AppendMessage(LocalizableText text)
    {
        if (LastMessage) LastMessage.Append(text);
    }

    public virtual void Clear()
    {
        foreach (var message in messages)
        {
            message.gameObject.SetActive(false);
            messagesPool.Push(message);
        }
        messages.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
        this.AssertRequiredObjects(messagesContainer, scrollRect, messagePrefab);

        inputManager = Engine.GetService<IInputManager>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        QuestLogEvents.Instance.OnQuestUpdated += AddMessage;

        if (inputManager.TryGetSampler(InputNames.ShowQuestLog, out var show))
            show.OnStart += Show;
        if (inputManager.TryGetSampler(InputNames.Cancel, out var cancel))
            cancel.OnEnd += Hide;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        QuestLogEvents.Instance.OnQuestUpdated -= AddMessage;

        if (inputManager.TryGetSampler(InputNames.ShowQuestLog, out var show))
            show.OnStart -= Show;
        if (inputManager.TryGetSampler(InputNames.Cancel, out var cancel))
            cancel.OnEnd -= Hide;
    }

    protected virtual void SpawnMessage(QuestLogMessage message)
    {
        var messageUI = default(QuestLogMessageUI);

        if (messages.Count > Capacity)
        {
            messageUI = messages.First.Value;
            messageUI.gameObject.SetActive(true);
            messageUI.transform.SetSiblingIndex(MessagesContainer.childCount - 1);
            messages.RemoveFirst();
            messages.AddLast(messageUI);
        }
        else
        {
            if (messagesPool.Count > 0)
            {
                messageUI = messagesPool.Pop();
                messageUI.gameObject.SetActive(true);
                messageUI.transform.SetSiblingIndex(MessagesContainer.childCount - 1);
            }
            else messageUI = Instantiate(MessagePrefab, MessagesContainer, false);

            messages.AddLast(messageUI);
        }

        messageUI.Initialize(message);
    }

    protected override void HandleVisibilityChanged(bool visible)
    {
        base.HandleVisibilityChanged(visible);

        MessagesContainer.gameObject.SetActive(visible);
        if (visible) ScrollToBottom();
    }

    protected override void SerializeState(GameStateMap stateMap)
    {
        base.SerializeState(stateMap);
        var state = new GameState
        {
            Messages = messages.TakeLast(SaveCapacity).Select(m => m.GetState()).ToList()
        };
        stateMap.SetState(state);
    }

    protected override async UniTask DeserializeState(GameStateMap stateMap)
    {
        await base.DeserializeState(stateMap);

        Clear();

        var state = stateMap.GetState<GameState>();
        if (state is null) return;

        if (state.Messages?.Count > 0)
            foreach (var message in state.Messages)
                SpawnMessage(message);
    }

    protected virtual async void ScrollToBottom()
    {
        await AsyncUtils.WaitEndOfFrameAsync();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ScrollRect.content);
        ScrollRect.verticalNormalizedPosition = 0;
    }

    protected override GameObject FindFocusObject()
    {
        var message = messages.Last;
        while (message != null)
        {
            if (message.Value.GetComponentInChildren<Selectable>() is Selectable selectable)
                return selectable.gameObject;
            message = message.Previous;
        }
        return null;
    }
}
