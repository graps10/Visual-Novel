using Naninovel;
using Naninovel.UI;

public class MapOpenButton : ScriptableButton
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
        uiManager.GetUI<MapUI>()?.Show();
    }
}
