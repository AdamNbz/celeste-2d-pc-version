using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairPart : MonoBehaviour
{
    public Transform target;
    public bool mainHairPart = false;

    public float lerpSpeed = 5f;
    public float maxDist = .5f;
    public float gravity = -.1f;
    
    private bool isReady = false;

    void Update()
    {
        if (!isReady) return;

        transform.position = new Vector3(transform.position.x, transform.position.y + gravity, transform.position.z);

        Vector2 diff = transform.position - target.position;
        Vector2 dir = diff.normalized;
        float dist = Mathf.Min(maxDist, diff.magnitude);

        Vector2 finalPos = (Vector2)target.position + dir * dist;

        Vector2 newPosLerped = Vector2.Lerp(transform.position, finalPos, Time.deltaTime * lerpSpeed);

        transform.position = newPosLerped;
    }
}
