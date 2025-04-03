using Naninovel;

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
