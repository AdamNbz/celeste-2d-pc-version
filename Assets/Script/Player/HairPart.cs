using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairPart : MonoBehaviour
{
    public Transform target;
    public SpriteRenderer spriteRenderer;
    
    [Header("Hair Settings")]
    public float maxDist = 0.08f;      // Maximum distance from target
    public Vector2 baseOffset = new Vector2(0f, -0.06f); // Default resting offset (down + slightly back)
    
    private PlayerController playerMovement;
    private bool isReady = false;
    private Vector2 environmentOffset = Vector2.zero; // Modified by wind, etc.
    private int hairIndex = 0;
    private float baseScale = 1f;

    public void InitHair(PlayerController movement, Transform targetTransform, int index, float scale)
    {
        this.playerMovement = movement;
        this.target = targetTransform;
        this.hairIndex = index;
        this.baseScale = scale;
        
        // Set the scale - hair gets smaller further from head
        transform.localScale = Vector3.one * baseScale;
        
        // Get sprite renderer for potential color changes (dash states, etc.)
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        isReady = true;
    }

    void Update()
    {
        if (!isReady || target == null) return;

        // Calculate the offset direction based on base offset + environment
        Vector2 desiredOffset = baseOffset + environmentOffset;
        
        // Get current position relative to target
        Vector2 currentPos = transform.position;
        Vector2 targetPos = target.position;
        
        // Calculate the desired position (target + offset)
        Vector2 desiredPos = targetPos + desiredOffset;
        
        // Move towards the desired position but respect max distance from target
        Vector2 toDesired = desiredPos - currentPos;
        
        // Smooth follow - the hair trails behind
        Vector2 newPos = Vector2.Lerp(currentPos, desiredPos, Time.deltaTime * 10f);
        
        // Clamp distance from target (the point we're following)
        Vector2 fromTarget = newPos - targetPos;
        if (fromTarget.magnitude > maxDist)
        {
            fromTarget = fromTarget.normalized * maxDist;
            newPos = targetPos + fromTarget;
        }

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    /// <summary>
    /// Set environment offset (for wind, underwater, etc.)
    /// </summary>
    public void SetEnvironmentOffset(Vector2 offset)
    {
        environmentOffset = offset;
    }

    /// <summary>
    /// Set the base offset (for animation-specific hair positions)
    /// </summary>
    public void SetBaseOffset(Vector2 offset)
    {
        baseOffset = offset;
    }
}
