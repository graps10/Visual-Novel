using System;
using Naninovel;
using UnityEngine;
using UnityEngine.Events;

public class QuestLogMessageUI : ScriptableUIBehaviour
{
    [Serializable]
    private class OnMessageChangedEvent : UnityEvent<string> { }

    protected virtual LocalizableText Text { get; private set; }

    [SerializeField] private OnMessageChangedEvent onMessageChanged;

    public virtual QuestLogMessage GetState() => new QuestLogMessage(Text);

    public virtual void Initialize(QuestLogMessage message)
    {
        SetText(message.Text);
    }

    public virtual void Append(LocalizableText text, string voicePath = null)
    {
        SetText(Text + text);
    }

    protected override void OnEnable() => base.OnEnable();

    protected override void OnDisable() => base.OnDisable();

    protected virtual void SetText(LocalizableText text)
    {
        Text = text;
        onMessageChanged?.Invoke(text);
    }
}
