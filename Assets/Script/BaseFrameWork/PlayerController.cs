using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerState state;
    Animator animator;
    int MoveStrength
    {
        get { return animator.GetInteger("MoveStrength"); } 
        set { animator.SetInteger("MoveStrength", value); }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        if (state != null)
        {
            state.FixedUpdate();
        }
    }
    
    public void SetState(PlayerState newState)
    {
        if(state != null)
        {
            newState.prevState = state;
            state.Exit();
        }
        state = newState;
    }
}
