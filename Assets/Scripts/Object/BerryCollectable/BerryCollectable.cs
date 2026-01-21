using Assets.Scripts.BaseFrameWork.States;
using Assets.Scripts.Object.BerryCollectable;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class BerryCollectable : MonoBehaviour
{
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
            Debug.Log("Berry's animator is null");
        }
        SetState(new Idle(this));
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
            gameObject.SetActive(false);
        }
    }

    private void HandleOnCollect()
    {
        SetState(new Collected(this));
    }

    public void SetData(StrawberryData data)
    {
        this.data = data;
    }
}
