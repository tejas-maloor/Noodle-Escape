using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinisherState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        isIdleState = false;
        attackIndex = 3;
        duration = 1.6f;
        animator.SetTrigger("Attack" + 3);
        Debug.Log("Player Attack" + 3 + " Fired");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {

            ComboCharacter.isIdleCombat = true;
            controller.canMove = true;
            stateMachine.SetNextStateToMain();
        }
    }
}
