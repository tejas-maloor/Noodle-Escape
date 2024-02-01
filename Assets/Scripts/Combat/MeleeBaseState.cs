using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeBaseState : State
{
    public float duration;
    protected Animator animator;
    protected bool shouldCombo;
    protected int attackIndex;
    protected PlayerController controller;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        isIdleState = false;
        animator = _stateMachine.anim;
        controller = _stateMachine.playerController;
        ComboCharacter.Attack += OnAttack;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

/*        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            shouldCombo = true;
        }*/
    }

    public void OnAttack()
    {
        shouldCombo = true;
    }
}
