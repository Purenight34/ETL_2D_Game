
public abstract class PlayerState : BaseState
{
    protected Player _player;
    protected PlayerStateMachine _stateMachine;

    public PlayerState(Player player, PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
 
}
