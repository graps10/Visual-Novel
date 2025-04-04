using System;
using Naninovel;
using UnityEngine;

[Serializable]
public struct QuestLogMessage : IEquatable<QuestLogMessage>
{
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
}
