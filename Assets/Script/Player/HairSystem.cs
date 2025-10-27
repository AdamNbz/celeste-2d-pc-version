using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HairObject : MonoBehaviour
{
    [SerializeField] List<HairPart> hairParts;
    [SerializeField] List<Vector3> offSets;
    [SerializeField] int NumberOfParts = 5;
    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < NumberOfParts; i++)
        {
            hairParts[i].gameObject.transform.position = hairParts[i - 1].transform.position + offSets[i];
        }
    }
}