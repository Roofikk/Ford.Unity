public class NormalState : ActionStateBase
{
    public override void Enter(ActionBone action)
    {
        action.SetColor(action.Colors.Default);
        action.Outline.enabled = false;
    }

    public override void Exit(ActionBone action)
    {
    }
}
