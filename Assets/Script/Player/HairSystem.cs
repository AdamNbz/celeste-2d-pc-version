using UnityEngine;
using System.Collections.Generic;

public class HairObject : MonoBehaviour
{
    [SerializeField] private List<HairPart> hairParts = new List<HairPart>();
    [SerializeField] private int numberOfParts = 5;
    
    [Header("Hair Settings")]
    [SerializeField] private float maxDistBetweenParts = 0.08f;
    [SerializeField] private Vector2 baseOffset = new Vector2(0f, -0.06f); // Down + slightly back
    [SerializeField] private float[] scalePerPart = { 1f, 0.9f, 0.8f, 0.6f, 0.4f }; // Circles get smaller
    
    private PlayerController playerController;
    private Transform hairAnchor;

    /// <summary>
    /// Initialize the hair system with the player and anchor point
    /// </summary>
    public void Initialize(PlayerController player, Transform anchor)
    {
        playerController = player;
        hairAnchor = anchor;
        
        // Get all hair parts as children
        hairParts.Clear();
        foreach (Transform child in transform)
        {
            HairPart part = child.GetComponent<HairPart>();
            if (part != null)
            {
                hairParts.Add(part);
            }
        }
        
        numberOfParts = hairParts.Count;
        
        // Initialize each hair part
        Transform previousTarget = hairAnchor;
        for (int i = 0; i < numberOfParts; i++)
        {
            float scale = (i < scalePerPart.Length) ? scalePerPart[i] : 0.3f;
            hairParts[i].maxDist = maxDistBetweenParts;
            hairParts[i].baseOffset = baseOffset;
            hairParts[i].InitHair(playerController, previousTarget, i, scale);
            previousTarget = hairParts[i].transform;
        }
    }

    /// <summary>
    /// Set environment offset for all hair parts (wind, underwater, etc.)
    /// </summary>
    public void SetEnvironmentOffset(Vector2 offset)
    {
        foreach (var part in hairParts)
        {
            part.SetEnvironmentOffset(offset);
        }
    }

    /// <summary>
    /// Set base offset for all hair parts (animation-specific positions)
    /// </summary>
    public void SetBaseOffset(Vector2 offset)
    {
        foreach (var part in hairParts)
        {
            part.SetBaseOffset(offset);
        }
    }

    /// <summary>
    /// Set hair color (for dash state changes - red when no dashes, etc.)
    /// </summary>
    public void SetHairColor(Color color)
    {
        foreach (var part in hairParts)
        {
            if (part.spriteRenderer != null)
            {
                part.spriteRenderer.color = color;
            }
        }
    }
}