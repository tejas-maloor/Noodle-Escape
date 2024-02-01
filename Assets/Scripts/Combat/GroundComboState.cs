using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComboState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        isIdleState = false;
        attackIndex = 2;
        duration = .5f;
        animator.SetTrigger("Attack" + 2);
        Debug.Log("Player Attack" + 2 + " Fired");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {
            if (shouldCombo)
            {
                ComboCharacter.isIdleCombat = false;
                stateMachine.SetNextState(new GroundFinisherState());
            }
            else
            {
                ComboCharacter.isIdleCombat = true;
                controller.canMove = true;
                stateMachine.SetNextStateToMain();
            }
        }
    }
}
