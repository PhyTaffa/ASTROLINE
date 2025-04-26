    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PathFollower : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private WPManager wpManager;
        [SerializeField] private GameObject startNode;
        [SerializeField] private GameObject endNode;
        [SerializeField] private float speed = 80f;
        [SerializeField] private float rotationSpeed = 10f;
        
        //to immediatly start a new path
        public bool autoLoopPaths = true;
        

        private List<Node> path;
        private int currentIndex = 0;
        private bool isMoving = false;

        private Rigidbody rb;
        [SerializeField] private float threshqold = 10f;

        public event System.Action OnPathFinished;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Start following path from startNode to endNode
        public void StartFollowingPath()
        {
            // if (wpManager.graph.AStar(startNode, endNode))
            // {
            //     path = wpManager.graph.pathList;
            //     if (path != null && path.Count > 0)
            //     {
            //         currentIndex = 0;
            //         isMoving = true;
            //     }
            // }
            currentIndex = 0;
            isMoving = true;
            RunPathfinding();
        }

        /// <summary>
        /// Called to stop the ai agent after it reaches the given end node
        /// </summary>
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

            // Rotate towards the target, keep in mind the axis of transform.up due to plaent inclination
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);
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
                    isMoving = false;
                    OnPathFinished?.Invoke();
                    
                    //GenerateNewPath();

                    // Reached the end, generate a new path
                    
                    //Invoke(nameof(GenerateNewPath), 1f);
                }
            }
        }
        
        public void GenerateNewPath()
        {
            if (endNode != null)
                startNode = endNode;

            GameObject[] waypoints = wpManager.waypoints;
            if (waypoints.Length < 2) return;

            GameObject newEnd;
            do
            {
                newEnd = waypoints[Random.Range(0, waypoints.Length)];
            } while (newEnd == startNode);

            endNode = newEnd;
            RunPathfinding();
        }

        public void GenerateNewPath(GameObject specificEnd)
        {
            if (endNode != null)
                startNode = endNode;

            endNode = specificEnd;
            RunPathfinding();
        }

        private void RunPathfinding()
        {
            if (wpManager.graph.AStar(startNode, endNode))
            {
                path = wpManager.graph.pathList;
                currentIndex = 0;

                if (autoLoopPaths)
                {
                    isMoving = true;
                }
            }
        }

        #region was working

                // Generate a new path with a random waypoint as the new end
                // public void GenerateNewPath()
                // {
                //     if (endNode != null)
                //     {
                //         startNode = endNode;  // The old endNode becomes the new startNode
                //     }
                //
                //     // Pick a random waypoint that isn't the current one
                //     GameObject[] waypoints = wpManager.waypoints;
                //     if (waypoints.Length < 2) return;
                //
                //     GameObject newEnd;
                //     do
                //     {
                //         newEnd = waypoints[Random.Range(0, waypoints.Length)];
                //     } while (newEnd == startNode);
                //
                //     endNode = newEnd;  // Set the new endNode
                //
                //     // Run A* again with the new startNode and endNode
                //     if (wpManager.graph.AStar(startNode, endNode))
                //     {
                //         path = wpManager.graph.pathList;
                //         currentIndex = 0;
                //         if (autoLoopPaths)
                //         {
                //             isMoving = true;
                //         }
                //         else
                //         {
                //             isMoving = false;
                //         }
                //     }
                // }

        #endregion


        // For use in other scripts to set start and end nodes
        public void SetStartNode(GameObject newStartNode)
        {
            startNode = newStartNode;
        }

        public void SetEndNode(GameObject newEndNode)
        {
            endNode = newEndNode;
        }

        #region Haram


        private void MoveJustToEndNode()
        {
            Transform target = path[currentIndex].GetID().transform;
            Vector3 targetPos = target.position;
            Vector3 direction = (targetPos - transform.position).normalized;

            // Rotate towards the target, keep in mind the axis of transform.up due to plaent inclination
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);
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
                    OnPathFinished?.Invoke();
                    // Reached the end, generate a new path
                    GenerateNewPathNoWalk();
                    //Invoke(nameof(GenerateNewPath), 1f);
                }
            }
        }
        
        void GenerateNewPathNoWalk()
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
            }
        }


        #endregion
        

        #region no clue

        public GameObject GetClosestWaypoint()
        {
            if (wpManager == null || wpManager.waypoints == null || wpManager.waypoints.Length == 0)
                return null;

            GameObject closest = null;
            float minDistance = Mathf.Infinity;
            Vector3 position = transform.position;

            foreach (GameObject wp in wpManager.waypoints)
            {
                float distance = Vector3.Distance(position, wp.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = wp;
                }
            }

            return closest;
        }

        #endregion
    }
