using Naninovel;
using Naninovel.UI;
using UnityEngine;

public class QuestLogOpenButton : ScriptableButton
{
    [SerializeField] private QuestNotificationMark notificationMark;
    private IUIManager uiManager;
    private IQuestLogManager questManager;

    protected override void Awake()
    {
        base.Awake();
        uiManager = Engine.GetService<IUIManager>();
        questManager = Engine.GetService<IQuestLogManager>();

        questManager.OnQuestNotification += ShowNotification;
    }

    protected override void OnDestroy()
    {
        if (questManager != null)
        {
            questManager.OnQuestNotification -= ShowNotification;
        }
        base.OnDestroy();
    }

    protected override void OnButtonClick()
    {
        uiManager.GetUI<IPauseUI>()?.Hide();
        uiManager.GetUI<IQuestLogUI>()?.Show();

        HideNotification();
    }

    private void ShowNotification() => notificationMark?.ShowMark();
    private void HideNotification() => notificationMark?.HideMark();
}
