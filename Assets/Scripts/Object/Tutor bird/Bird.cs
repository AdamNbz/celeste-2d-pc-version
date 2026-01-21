using UnityEngine;

public class Bird : MonoBehaviour
{ 
    private void OnEnable()
    {
        ResetBird();
    }

    enum State
    {
        Idle,
        FlyingUp,
        Flying
    }

    private State currentState = State.Idle;
    private bool isIdle = false;
    private float flyUpSpeed = 2f;
    private float flySpeed = 5f;
    private float flyUpHeight = 3f;

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (!isIdle)
                {
                    // Idle behavior here
                    isIdle = true;
                }
                break;
            case State.FlyingUp:
                if (GetComponent<Animator>().GetComponent<Animator>() != null)
                {
                    GetComponent<Animator>().Play("FlyingUp");
                }
                transform.Translate(Vector3.up * flyUpSpeed * Time.deltaTime);
                transform.Translate(Vector3.right * flySpeed * Time.deltaTime);
                if (transform.position.y >= flyUpHeight)
                {
                    currentState = State.Flying;
                }
                break;
            case State.Flying:
                transform.Translate(Vector3.right * flySpeed * Time.deltaTime);
                break;
        }
    }
    
    public void Fly()
    {
        currentState = State.FlyingUp;
        isIdle = false;
    }

    public void ResetBird()
    {
        currentState = State.Idle;
        isIdle = false;
        transform.position = transform.parent.Find("FlyTrigger").position;
    }

    public void OnTextVanishComplete()
    {
       gameObject.SetActive(false);
    }

    public void OnFlyingUpAnimationComplete()
    {
        currentState = State.Flying;
        GetComponent<Animator>().Play("Flying");
    }
}
