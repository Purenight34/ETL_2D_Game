using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_IdleState : PlayerState
{
    public P_IdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void OnEnter()
    {
    }


    public override void OnUpdate()
    {
        _player.Animator.SetFloat("Horizontal", _player.Move());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("점프시작");
            if (_player.JumpStart())
                _stateMachine.OnStateChange(_stateMachine.JumpStartState);
        }

        else if (!_player.GroundDetection())
        {
            _stateMachine.OnStateChange(_stateMachine.JumpStartState);
        }

    }


    public override void OnFixedUpdate()
    {
    }


    public override void OnExit()
    {
    }
}
