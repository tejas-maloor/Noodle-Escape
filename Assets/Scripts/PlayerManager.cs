using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject player;
    public PlayerController playerController;
    public StateMachine stateMachine;
    public bool isDead = false;
}


