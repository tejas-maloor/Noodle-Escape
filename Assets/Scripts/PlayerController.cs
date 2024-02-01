using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if(instance == null)
            {
				instance = FindObjectOfType<PlayerController>();
			}
			return instance;
        }
    }

    public static event Action jumpPerformed;

    public float turnSpeed = 10f;
    public bool canMove = true;

    [Header("References")]
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] NoodleBar noodleBar;
 
    [Header("Movement")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float slowDownTime;

    [Header("Jump")]
    [SerializeField] float jumpPower;
    [SerializeField] float groundDistance;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float groundingForce;
    [SerializeField] float fallAcceleration;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float gravityModifier;

    [Header("Roll")]
    [SerializeField] float movingRollAmount;
    [SerializeField] float idleRollAmount;
    [SerializeField] float coolDown;

    [Header("Dialogues")]
    public AudioClip kickDialogue;
    public AudioClip hitSound;
    public AudioClip slurpSound;

    private Vector2 move;
    private Vector3 velocity;
    private Vector3 movement;
    private bool grounded;
    private bool endedJumpEarly;
    private bool jumpToConsume;
    private bool rollToConsume;
    private bool jumpKeyHeld;
    private float currentSpeed;
    private float lastRolledAt = -9999f;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    
    public void OnJumpDown(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            jumpToConsume = true;
            jumpKeyHeld = true;
        }

        if(context.canceled)
        {
            jumpKeyHeld = false;
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(Time.time > lastRolledAt + coolDown)
            {
                rollToConsume = true;
                lastRolledAt = Time.time;
            }
        }
    }

    private void Start()
    {
        currentSpeed = maxSpeed;
    }

    private void Update()
    {
        if(canMove)
        {
            movement = new Vector3(move.x, 0, move.y);
        }
        else
        {
            movement = Vector3.zero;
        }
        HandleRotation();

        currentSpeed = Mathf.MoveTowards(currentSpeed, 0, slowDownTime * Time.deltaTime);
        noodleBar.UpdateBar(currentSpeed, maxSpeed);

        if(currentSpeed <= 0)
        {
            anim.SetBool("Dead", true);
            PlayerManager.instance.isDead = true;
            Invoke(nameof(GoToGameOver), 1f);
        }

        if(transform.position.y <= -8)
        {
            PlayerManager.instance.isDead = true;
            Invoke(nameof(GoToGameOver), 0);
        }

    }

    private void FixedUpdate()
    {
        CheckCollisions();

		HandleMovement();
		HandleRoll();
		HandleJump();
        HandleGravity();

        //Debug.Log(grounded);
        rb.velocity = velocity;
    }

    private void CheckCollisions()
    {
        bool groundHit = Physics.CheckSphere(groundCheck.position, groundDistance, ~playerLayer);

        if(!grounded && groundHit)
        {
            grounded = true;
        }
        else if(grounded && !groundHit)
        {
            grounded = false;
        }
    }

    public void HandleMovement()
    {
        if(movement == Vector3.zero)
        {
            velocity = Vector3.MoveTowards(velocity, new Vector3(0, rb.velocity.y, 0), deceleration * Time.deltaTime);
        }
        else
        {
            Vector3 targetPos = movement.ToIso() * movement.normalized.magnitude * currentSpeed;

            Vector3 newVelocity = new Vector3(targetPos.x, rb.velocity.y, targetPos.z);

            //velocity = (Vector3.Distance(newVelocity, velocity) > 10f) ? newVelocity :  Vector3.MoveTowards(velocity, newVelocity, acceleration * Time.deltaTime);

            velocity = Vector3.MoveTowards(velocity, new Vector3(targetPos.x, rb.velocity.y, targetPos.z), acceleration * Time.deltaTime);
        }

        anim.SetFloat("Speed", move.normalized.sqrMagnitude, 0.05f, Time.deltaTime);
    }

    private void HandleRoll()
    {
        if(rollToConsume && grounded)
        {
            if(movement == Vector3.zero)
            {
                velocity = transform.GetChild(0).transform.forward * idleRollAmount;
            }
            else
            {
                velocity = movement.ToIso() * movement.normalized.magnitude * movingRollAmount;
            }

            anim.SetTrigger("Roll");

            rollToConsume = false;
        }
    }

    private void HandleGravity()
    {
        if(grounded && velocity.y <= 0f)
        {
            velocity.y = groundingForce;
        }
        else
        {
            var inAirGravity = fallAcceleration;
            if (endedJumpEarly && velocity.y > 0) inAirGravity *= gravityModifier;
            velocity.y = Mathf.MoveTowards(velocity.y, -maxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }

    public void HandleJump()
    {
        if (!endedJumpEarly && !grounded && !jumpKeyHeld && rb.velocity.y > 0) endedJumpEarly = true;

        if (!jumpToConsume) return;

        if (grounded) ExecuteJump();

        jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        endedJumpEarly = false;
        anim.SetTrigger("Jump");
        velocity.y = jumpPower;
        jumpPerformed?.Invoke();
    }

    void HandleRotation()
    {
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            //transform.forward = Vector3.Lerp(transform.forward, movement.normalized, Time.deltaTime * turnSpeed);
        }
    }

    public void RestoreSpeed()
    {
        SoundManager.instance.PlaySFX(slurpSound);
        currentSpeed += maxSpeed / 2;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    public void TakeDamage()
    {
        //Debug.Log("Hit Player");
        anim.SetTrigger("Hit");
        currentSpeed -= 0.5f;
    }

    void GoToGameOver()
    {
        SceneManager.LoadScene(2);
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
