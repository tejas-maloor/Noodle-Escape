using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboCharacter : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private StateMachine meleeStateMachine;

    public static event Action Attack;
    public static bool isIdleCombat = true;

    private void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Attack?.Invoke();

            if(isIdleCombat)
            {
                Debug.Log("Enter Combo");
                isIdleCombat = false;
                playerController.canMove = false;
                meleeStateMachine.SetNextState(new GroundEntryState());
            }
        }
    }
}
