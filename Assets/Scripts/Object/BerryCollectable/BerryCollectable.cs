using Assets.Scripts.BaseFrameWork.States;
using Assets.Scripts.Object.BerryCollectable;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class BerryCollectable : MonoBehaviour
{
    string berryId;
    Animator animator = null;
    BerryState state;
    BerryState nextState;
    StrawberryData data;
    public Animator Animator
    {
        get { return animator; }
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("Berry's animator is null");
        }
        SetState(new Idle(this));
        data = new StrawberryData(berryId);
    }

    public void SetState(BerryState newState)
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

    public BerryState GetState()
    {
        return state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            HandleOnCollect();
            data.SetCollect(true);
        }
    }

    private void HandleOnCollect()
    {
        SetState(new Collected(this));
    }
}
