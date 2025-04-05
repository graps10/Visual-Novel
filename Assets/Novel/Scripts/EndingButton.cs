using Naninovel;
using Naninovel.UI;

public class EndingButton : ScriptableButton
{
    private IStateManager gameState;
    private IUIManager uiManager;

    protected override void Awake()
    {
        base.Awake();

        gameState = Engine.GetService<IStateManager>();
        uiManager = Engine.GetService<IUIManager>();
    }

    protected override void OnButtonClick()
    {
        uiManager.GetUI<IPauseUI>()?.Hide();

        ExitToTitleAsync();
    }

    private async void ExitToTitleAsync()
    {
        await gameState.ResetStateAsync();
        uiManager.GetUI<ITitleUI>()?.Show();
    }
}