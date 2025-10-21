using NUnit.Framework;
using UnityEngine;

public class HairObject : MonoBehaviour
{
    [SerializeField] HairPart[] hairPart;
    [SerializeField] int NumberOfParts = 5;
    [SerializeField] Vector3 Maxoffset;
    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < NumberOfParts; i++)
        {
            hairPart[i].gameObject.transform.position = hairPart[i - 1].transform.position + Maxoffset / NumberOfParts;
        }
    }
}