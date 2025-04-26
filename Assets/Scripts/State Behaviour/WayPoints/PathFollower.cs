using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private WPManager wpManager;
    [SerializeField] private GameObject startNode;
    [SerializeField] private GameObject endNode;
    [SerializeField] private float speed = 80f;
    [SerializeField] private float rotationSpeed = 10f;

    private List<Node> path;
    private int currentIndex = 0;
    private bool isMoving = false;

    private Rigidbody rb;
    [SerializeField] private float threshqold = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start following path from startNode to endNode
    public void StartFollowingPath()
    {
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

    // Stop following the current path
    public void StopFollowingPath()
    {
        isMoving = false;
    }

    void Update()
    {
        if (isMoving && path != null && currentIndex < path.Count)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        Transform target = path[currentIndex].GetID().transform;
        Vector3 targetPos = target.position;
        Vector3 direction = (targetPos - transform.position).normalized;

        // Rotate towards the target
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f));
        }

        // Move towards the target
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        rb.MovePosition(newPosition);

        // Proceed to next node if close enough
        if (Vector3.Distance(transform.position, targetPos) < threshqold)
        {
            currentIndex++;
            if (currentIndex >= path.Count)
            {
                // Reached the end, generate a new path
                GenerateNewPath();
            }
        }
    }

    // Generate a new path with a random waypoint as the new end
    void GenerateNewPath()
    {
        if (endNode != null)
        {
            startNode = endNode;  // The old endNode becomes the new startNode
        }

        // Pick a random waypoint that isn't the current one
        GameObject[] waypoints = wpManager.waypoints;
        if (waypoints.Length < 2) return;

        GameObject newEnd;
        do
        {
            newEnd = waypoints[Random.Range(0, waypoints.Length)];
        } while (newEnd == startNode);

        endNode = newEnd;  // Set the new endNode

        // Run A* again with the new startNode and endNode
        if (wpManager.graph.AStar(startNode, endNode))
        {
            path = wpManager.graph.pathList;
            currentIndex = 0;
            isMoving = true;
        }
    }

    // For use in other scripts to set start and end nodes
    public void SetStartNode(GameObject newStartNode)
    {
        startNode = newStartNode;
    }

    public void SetEndNode(GameObject newEndNode)
    {
        endNode = newEndNode;
    }
}
