using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIManager : MonoBehaviour
{
    [SerializeField] private List<Transform> pois = new List<Transform>();

    public Transform GetPOIAtIndex(int index)
    {
        if (index >= pois.Count) 
            return null;
        
        return pois[index];
    }

    public bool IsIndexValid(int index)
    {
        return index < pois.Count && index >= 0;
    }

}
