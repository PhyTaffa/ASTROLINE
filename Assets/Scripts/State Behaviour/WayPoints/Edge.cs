using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node startNode;
    public Node endNode;

    public Edge(Node start, Node end)
    {
        this.startNode = start;
        this.endNode = end;
    }
}
