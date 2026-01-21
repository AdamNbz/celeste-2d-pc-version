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
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (!isIdle)
                {
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
    
    /// <summary>
    /// Make the bird face towards a target position (flip horizontally)
    /// </summary>
    public void LookAtTarget(Vector3 targetPosition)
    {
        // If target is to the left of bird, flip to face left (negative scale)
        // If target is to the right, face right (positive scale)
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    /// <summary>
    /// Reset bird to face original direction (right)
    /// </summary>
    public void FaceOriginalDirection()
    {
        transform.localScale = originalScale;
    }
    
    public void Fly()
    {
        // Face original direction (right) before flying
        FaceOriginalDirection();
        currentState = State.FlyingUp;
        isIdle = false;
        
        // Play fly away sound effect
        if (gameObject.activeInHierarchy) AudioManager.Instance.PlayPlayerSFX("baby_flyaway_01");
    }

    public void ResetBird()
    {
        currentState = State.Idle;
        isIdle = false;
        transform.localScale = originalScale;
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
