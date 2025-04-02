using System;
using Naninovel;
using UnityEngine;

/// <summary>
/// Represents a message recorded in <see cref="IQuestLogUI"/>.
/// </summary>
[Serializable]
public struct QuestLogMessage : IEquatable<QuestLogMessage>
{
    /// <summary>
    /// Text of the message.
    /// </summary>
    public LocalizableText Text => text;

    [SerializeField] private LocalizableText text;


    public QuestLogMessage(LocalizableText text)
    {
        this.text = text;
    }

    public bool Equals(QuestLogMessage other)
    {
        return text.Equals(other.text);
    }

    public override bool Equals(object obj)
    {
        return obj is QuestLogMessage other && Equals(other);
    }

    public override int GetHashCode()
    {
        return text.GetHashCode();
    }
}
