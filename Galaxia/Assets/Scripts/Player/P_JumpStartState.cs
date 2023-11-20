using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_JumpStartState : PlayerState
{
    private float _stateChangeTimer = 0.1f;


    public P_JumpStartState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void OnEnter()
    {
        _player.Animator.SetBool("IsJump", true);
        _stateChangeTimer = 0.1f;
    }

    public override void OnUpdate()
    {
        _player.Animator.SetFloat("Horizontal", _player.Move());


        _stateChangeTimer -= Time.deltaTime;
        if (_stateChangeTimer > 0)
            return;

        if (_player.GroundDetection())
            _stateMachine.OnStateChange(_stateMachine.IdleState);

    }


    public override void OnFixedUpdate()
    {
    }


    public override void OnExit()
    {
        _player.ResetJumpCount();
        _player.Animator.SetBool("IsJump", false);
        _stateChangeTimer = 0.1f;

    }
}
