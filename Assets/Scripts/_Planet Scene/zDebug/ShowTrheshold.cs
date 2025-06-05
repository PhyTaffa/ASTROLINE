using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTrheshold : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 4);
    }
}
