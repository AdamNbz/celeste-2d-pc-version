using Enermy_State;
using UnityEngine;
public class EnermyController: MonoBehaviour
{
    EnermyState state;
    Animator animator;
    EnermyState nextState;

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
        if (nextState != state && nextState != null)//do this for only one enter exit once per frame
        {
            state.Exit();
            nextState.prevState = state;
            state = nextState;
            state.Enter();
        }
        else
        {
            Debug.Log("Next state is null");
        }
        if (state != null)
        {
            state.FixedUpdate();
        }
    }

    public void SetState(EnermyState newState)
    {
        if (state != null)
        {
            nextState = newState;
        }
        else
        {
            state = newState;
            state.Enter();
        }
    }
    //apply control here or in the state
}