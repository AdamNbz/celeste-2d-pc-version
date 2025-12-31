using Assets.Script.Player.States;
using Assets.Script.Player.VFX;
using Assets.Script.SaveData;
using NUnit.Framework;
using Player_State;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviour
{
    
    [Header("Input")]
    [SerializeField] InputAction moveAction;
    [SerializeField] InputAction Jump;
    [SerializeField] InputAction Dash;

    [Header("Movement Settings")]
    [SerializeField] float movementSpeed = 10;
    [SerializeField] float jumpForce = 10;

    [Header("Wall Climb Settings")]
    [SerializeField] float maxClimbTime = 1.2f;   // tClimb
    [SerializeField] float wallClimbCooldown = 0.5f; // t
   
    [Header("Foot and Hand Object")]
    [SerializeField] Transform footPosition;
    [SerializeField] Transform handPosition;


    // private fields
    PlayerData data;
    bool isDeathed = false;
    float wallCooldownTimer;
    PlayerState state;
    Animator animator;
    Rigidbody2D rb;
    // private component
    LandingEffect landingEffect;
    Vector2 originalScale;
    public PlayerState nextState;
    int _Direction = 1;
    float currentSpeed = 5;
    
    public void SetPlayerData(PlayerData data)
    {
        this.data = data;
    }

    public PlayerData GetPlayerData()
    {
        return data;
    }


    public int Direction
    {
        get { return _Direction; }
        set {
            _Direction = value;
            transform.localScale = new Vector2(Direction * originalScale.x, originalScale.y);
        }
    }
    private void OnEnable()
    {
        moveAction.Enable();
        Jump.Enable();
        Dash.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        Jump.Disable();
        Dash.Disable();
    }

    private void Start()
    {
        Debug.Log("PlayerController Start");
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on PlayerController.");
        }
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on PlayerController.");
        }
        SetState(new Idle(this));
        originalScale = transform.localScale;
        if(originalScale == null)
        {
            Debug.LogError("Original Scale could not be determined.");
        }
        landingEffect = GetComponent<LandingEffect>();
    }

    private void Update()
    {
        if(state != null)
        {
            state.Update();
        }
        
    }
    private void FixedUpdate()
    {
        
        if (wallCooldownTimer > 0)
            wallCooldownTimer -= Time.fixedDeltaTime;

        if (nextState != state && nextState != null && nextState.GetType() != state.GetType())//do this for only one enter exit once per frame
        {
            state.Exit();
            nextState.prevState = state;
            state = nextState;
            state.Enter();
        }
        else if(nextState==null)
        {
            Debug.Log("Next state is null");
        }
        if (state != null)
        {
            state.FixedUpdate();
        }
        
    }

    public void SetState(PlayerState newState)
    {
        if (state != null)
        {
            nextState = newState;
        }
        else//the first state
        {
            state = newState;
            state.Enter();
        }
    }

    public PlayerState GetState()
    {
        return state;
    }
    public Animator GetAnimator()
    {
        return animator;
    }
    // Can Aplly Control Hear Or In The State
    public bool HandleMovement()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        if (inputVector.x != 0)
        {
            Direction = inputVector.x > 0 ? 1 : -1;
            currentSpeed = movementSpeed;
            rb.linearVelocity=new Vector2(currentSpeed*Direction,rb.linearVelocity.y);
            return true;
        }
        else
        {
            rb.linearVelocity=new Vector2(0, rb.linearVelocity.y);
            currentSpeed = 0;
        }
        return false;
    }
    
    public bool HandleJump()
    {
       
        if (Jump.IsPressed()&& IsOnTheGround() && state.GetStateName() != "Jump")
        {
           
            rb.linearVelocity += new Vector2(0, jumpForce);
            SetState(new Jump(this));
            return true;
        }
        return false;
    }

    public bool IsOnTheGround()
    {
        if(footPosition==null)
        {
            Debug.LogError("Foot Position is null in IsOnTheGround");
            return false;
        }
        return Physics2D.OverlapCircle(footPosition.position, 0.1f, 1 << LayerMask.NameToLayer("Ground"));
    }

    public bool IsTouchingWall()
    {
        return Physics2D.OverlapCircle(handPosition.position, 0.1f, 1 << LayerMask.NameToLayer("Ground"));
    }

    public Vector2 GetObjectVelocity()
    {
        return rb.linearVelocity;
    }

    public void SetObjectVelocity(float x,float y)
    {
        rb.linearVelocity = new Vector2(x, y);
    }
    
    public void HandleDash()
    {
        if(Dash.IsPressed())
        {
            rb.linearVelocity = new Vector2(Direction * movementSpeed * 2, rb.linearVelocity.y);
            SetState(new Dash(this));
        }
    }

    public void SpawnLandingEffect()
    {

        landingEffect.SpawnLandingEffect(footPosition.position);
    }

    public float dashSpeed
    {
        get { return movementSpeed * 2; }
    }

    public float MaxClimbTime { get => maxClimbTime;}
    public float WallClimbCooldown { get => wallClimbCooldown; }

    public float GetMoveInputX()
    {
        return moveAction.ReadValue<Vector2>().x;
    }

    public bool IsPressingTowardWall()
    {
        float inputX = GetMoveInputX();

        if (inputX == 0) return false;

        // đang nhấn về phía đang quay mặt
        return Mathf.Sign(inputX) == Direction;
    }

    public bool CanClimbWall()
    {
        return wallCooldownTimer <= 0;
    }

    public void StartWallCooldown()
    {
        wallCooldownTimer = wallClimbCooldown;
    }


    public void SetPlayerPosition(Vector2 newPos)
    {
        transform.position = newPos;
    }

    public void DisableInput()
    {
        moveAction.Disable();
        Jump.Disable();
        Dash.Disable();
    }

    public void EnableInput()
    {
        moveAction.Enable();
        Jump.Enable();
        Dash.Enable();
    }
    public void Death()
    {
        if(isDeathed)
        {
            return;
        }
        isDeathed = true;
        GameManager.GetInstance().OnPlayerDeath();
    }
}
