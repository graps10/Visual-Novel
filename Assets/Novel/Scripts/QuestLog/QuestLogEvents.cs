using System;
using Naninovel;
using UnityEngine;

public class QuestLogEvents : MonoBehaviour
{
    public static QuestLogEvents Instance { get; private set; }

    public event Action<LocalizableText> OnQuestUpdated;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddQuestMessage(LocalizableText text)
    {
        OnQuestUpdated?.Invoke(text);
    }
}