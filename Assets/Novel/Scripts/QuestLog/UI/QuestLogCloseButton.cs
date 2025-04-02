
using Naninovel;
using Naninovel.UI;

public class QuestLogCloseButton : ScriptableLabeledButton
{
    private QuestLogPanel questLogPanel;

    protected override void Awake()
    {
        base.Awake();

        questLogPanel = GetComponentInParent<QuestLogPanel>();
    }

    protected override void OnButtonClick() => questLogPanel.Hide();
}
