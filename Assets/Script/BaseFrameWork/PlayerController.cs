using UnityEngine;
using Player_State;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using NUnit.Framework;
using System;
public class PlayerController : MonoBehaviour
{
    PlayerState state;
    Animator animator;
    Rigidbody2D rb;
    [SerializeField] InputAction moveAction;
    [SerializeField] InputAction Jump;
    [SerializeField] InputAction Dash;
    [SerializeField] float movementSpeed;
    PlayerState nextState;
    int _Direction = 1;
    float currentSpeed = 5;
    public int Direction
    {
        get { return _Direction; }
        set {
            _Direction = value;
            transform.localScale = new Vector2(Direction,transform.localScale.y);
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        SetState(new Idle(this));
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
        if(nextState!=state&&nextState!=null)//do this for only one enter exit once per frame
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
        if (Jump.IsPressed()&& rb.IsTouchingLayers(LayerMask.NameToLayer("Ground")) && state.GetStateName() != "Jump")
        {
            rb.linearVelocity += new Vector2(0, 10);
            SetState(new Jump(this));
            return true;
        }
        return false;
    }

    public bool IsOnTheGround()
    {
        return rb.IsTouchingLayers(LayerMask.NameToLayer("Ground"));
    }

    public Vector2 GetObjectVelocity()
    {
        return rb.linearVelocity;
    }

    public void SetObjectVelocity(float x,float y)
    {
        Debug.Log(x + " " + y);
        rb.linearVelocity = new Vector2(x, y);
    }
    
    public void HandleDash()
    {
        if(Dash.IsPressed())
        {
            SetState(new Dash(this));
        }
    }
}
