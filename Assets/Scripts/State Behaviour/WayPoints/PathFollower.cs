using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private WPManager wpManager;
    [SerializeField] private GameObject startNode;
    [SerializeField] private GameObject endNode;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    private List<Node> path;
    private int currentIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        //GenerateNewPath();
        if (wpManager.graph.AStar(startNode, endNode))
        {
            path = wpManager.graph.pathList;
            if (path != null && path.Count > 0)
            {
                currentIndex = 0;
                isMoving = true;
            }
        }
    }

    void Update()
    {
        if (isMoving && path != null && currentIndex < path.Count)
        {
            Transform target = path[currentIndex].GetID().transform;
            Vector3 targetPos = target.position;
            Vector3 direction = (targetPos - transform.position).normalized;

            // Rotate toward target
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
            }

            // Move toward target
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            // Proceed to next node if close enough
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                currentIndex++;
                if (currentIndex >= path.Count)
                {
                    isMoving = false;
                    Invoke(nameof(GenerateNewPath), 1f); // Optional: small delay before new path
                }
            }
        }
    }

    void GenerateNewPath()
    {
        if (endNode != null)
        {
            startNode = endNode;
        }

        // Pick a random waypoint that isn't the current one
        GameObject[] waypoints = wpManager.waypoints;
        if (waypoints.Length < 2) return;

        GameObject newEnd;
        do
        {
            newEnd = waypoints[Random.Range(0, waypoints.Length)];
        } while (newEnd == startNode);

        endNode = newEnd;

        // Run A* again
        if (wpManager.graph.AStar(startNode, endNode))
        {
            path = wpManager.graph.pathList;
            currentIndex = 0;
            isMoving = true;
        }
    }
}
