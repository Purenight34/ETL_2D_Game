using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    private Player _player;


    public BaseState IdleState { get; private set; }

    public BaseState WalkState { get; private set; }

    public BaseState JumpStartState { get; private set; }

    public BaseState FallState { get; private set; }

    public BaseState CurrentState { get; private set; }


    public PlayerStateMachine(Player player)
    {
        _player = player;
        StateInit();
    }

    public override void OnStateUpdate()
    {
        CurrentState?.OnUpdate();
    }

    public override void OnStateFixedUpdate()
    {
        CurrentState?.OnFixedUpdate();
        Debug.Log(CurrentState);
    }

    private void StateInit()
    {
        IdleState = new P_IdleState(_player, this);
        WalkState = new P_WalkState(_player, this);
        JumpStartState = new P_JumpStartState(_player, this);

        CurrentState = IdleState;
    }


    public void OnStateChange(BaseState nextState)
    {
        if (CurrentState == nextState)
        {
            Debug.LogWarning("현재와 같은 상태로 전이할 수 없습니다.");
            return;
        }

        CurrentState?.OnExit();
        CurrentState = nextState;
        CurrentState?.OnEnter();
    }
}
