using Naninovel;
using Naninovel.UI;

public class QuestlLogOpenButton : ScriptableButton
{
    private IUIManager uiManager;

    protected override void Awake()
    {
        base.Awake();

        uiManager = Engine.GetService<IUIManager>();
    }

    protected override void OnButtonClick()
    {
        uiManager.GetUI<IPauseUI>()?.Hide();
        uiManager.GetUI<IQuestLogUI>()?.Show();
    }
}
