using UnityEngine;

public class ConveyorBlock : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    float direction = 1f;
    [SerializeField] GameObject startwaypoint;
    [SerializeField] GameObject otherwaypoint;
    [SerializeField] float boostforce = 2f;
    [SerializeField]private GameObject minwaypoint;
    [SerializeField]private GameObject maxwaypoint;
    private bool isMoving=false;
    private bool isCollisionWithPlayer = false;
    private Rigidbody2D rb;
    private void Start()
    {
        if (startwaypoint != null)
        {
            if (startwaypoint.transform.position.x < transform.position.x)
            {
                transform.position= startwaypoint.transform.position;
            }
        }
        if (otherwaypoint != null)
        {
            if (otherwaypoint.transform.position.x <startwaypoint.transform.position.x)
            {
                direction = -1f;
                minwaypoint = otherwaypoint;
                maxwaypoint = startwaypoint;
            }
            else
            {
                direction = 1f;
                minwaypoint = startwaypoint;
                maxwaypoint = otherwaypoint;
            }
        }
        rb= GetComponent<Rigidbody2D>();
    }

    private float calculatemovedratio()
    {
        if (direction > 0)
        {
            return (transform.position.x - minwaypoint.transform.position.x) / (maxwaypoint.transform.position.x - minwaypoint.transform.position.x);
        }
        else
        {
            return (maxwaypoint.transform.position.x - transform.position.x) / (maxwaypoint.transform.position.x - minwaypoint.transform.position.x);
        }
    }

    public float calculateboost()
    {
       return boostforce * calculatemovedratio();
    }

    private void FixedUpdate()
    {
        if (otherwaypoint!=null&&startwaypoint!=null)
        {
            if(isMoving)
            {
                transform.position+= new Vector3(direction * speed * Time.fixedDeltaTime, 0f, 0f);
            }
        }
        if (direction > 0)
        {
            if (transform.position.x >= maxwaypoint.transform.position.x)
            {
                isMoving = false;
                direction *= -1f;
                transform.position = maxwaypoint.transform.position;
            }
        }
        else
        {
            if (transform.position.x <= minwaypoint.transform.position.x)
            {
                isMoving = false;
                direction *= -1f;
                transform.position = minwaypoint.transform.position;
            }
        }

        if (isCollisionWithPlayer&&isMoving)
        {
            PlayerController player = GameManager.GetInstance().GetPlayerController();
            player.SetPlayerPosition(player.gameObject.transform.position + new Vector3(direction * speed * Time.fixedDeltaTime, 0f, 0f));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !(GetComponent<SpriteRenderer>().bounds.min.y+0.2 > collision.gameObject.GetComponent<SpriteRenderer>().bounds.max.y))
        {
           isMoving = true;
           isCollisionWithPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.linearVelocityX += calculateboost();
            isCollisionWithPlayer = false;
        }
    }
}
