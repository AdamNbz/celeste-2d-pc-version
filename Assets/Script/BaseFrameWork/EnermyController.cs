using Enermy_State;
using UnityEngine;
public class EnermyController: MonoBehaviour
{
    EnermyState state;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //SetState(new Idle(this));//set default state here exmaple for idle here
    }

    private void Update()
    {
        if (state != null)
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

    public void SetState(EnermyState newState)
    {
        if (state != null)
        {
            newState.prevState = state;
            state.Exit();
        }
        state = newState;
        state.Enter();
    }
    //apply control here or in the state
}