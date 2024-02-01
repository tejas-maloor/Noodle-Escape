using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEntryState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        isIdleState = false;
        attackIndex = 1;
        duration = .5f;
        animator.SetTrigger("Attack" + 1);
        Debug.Log("Player Attack" + 1 + " Fired");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(fixedtime >= duration)
        {
            if(shouldCombo)
            {
                ComboCharacter.isIdleCombat = false;
                stateMachine.SetNextState(new GroundComboState());
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
