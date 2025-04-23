using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private string tagObjectToFind = "";

    private Transform playerTransform = null;

    private bool hasSeenPlayerThisFrame = false;

    private void Awake()
    {
        // Find the first player as this should be the only one in the level anyways
        playerTransform = GameObject.FindGameObjectWithTag(tagObjectToFind)?.GetComponent<Transform>();
    }

    private void Update()
    {
        const float visionConeEulerAngles = 45.0f;
        float radVisionCone = visionConeEulerAngles * Mathf.Deg2Rad;

        Vector3 forwardVector = transform.forward;
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        float dotProduct = Vector3.Dot(forwardVector, directionToPlayer);

        if (dotProduct < radVisionCone)
        {
            hasSeenPlayerThisFrame = false;
            return;
        }

        Vector3 directionalMagnitudeToPlayer = (playerTransform.position - transform.position);
        float distanceToHeadset = directionalMagnitudeToPlayer.magnitude;
        Ray ray = new Ray(transform.position, directionalMagnitudeToPlayer.normalized);
        RaycastHit hitInfo;

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distanceToHeadset, Color.red, 10.0f) ;

        if (Physics.Raycast(ray, out hitInfo, distanceToHeadset))
        {
            if (hitInfo.collider.gameObject.CompareTag(tagObjectToFind))
            {
                hasSeenPlayerThisFrame = true;
                return;
            }
        }
        hasSeenPlayerThisFrame = false;
    }

    public bool HasSeenPlayerThisFrame()
    {
        return hasSeenPlayerThisFrame;
    }
}
























// TODO: Complete porting this to the new project

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// 
// public class MonsterLineOfSight : MonoBehaviour
// {
//     [SerializeField] private float visionDistance = 5.0f;
//     [SerializeField] private float viewConeAngle = 45.0f;
// 
//     [SerializeField] private Transform playerTransform = null;
// 
//     //private MonsterBehaviour monsterBehaviour = null;
// 
//     private ColourChanger colourChanger = null;
// 
//     // Start is called before the first frame update
//     void Start()
//     {
//         colourChanger = GetComponent<ColourChanger>();
// 
//         monsterBehaviour = GetComponent<MonsterBehaviour>();
//     }
// 
//     // Update is called once per frame
//     void Update()
//     {
//         float lineRange = 3;
// 
//         // Translate Forward to 45 degrees
//         // https://stackoverflow.com/questions/14607640/rotating-a-vector-in-3d-space
//         //x' = x cos θ − y sin θ
//         //y' = x sin θ + y cos θ
// 
//         // Forward Vector is (0, 0, 1) (simplified example) as we rotate around y we then use the following matrix
//         // | cos θ    0   sin θ | | x |   | x cos θ + z sin θ|   | x'|
//         // | 0        1       0 | | y | = |           y      | = | y'|
//         // |−sin θ    0   cos θ | | z |   |−x sin θ + z cos θ|   | z'|
// 
//         Vector3 forward = transform.forward;
//         float angle = viewConeAngle;
//         //         float rotatedX = forward.x * Mathf.Cos(45 * Mathf.Deg2Rad) + forward.z * Mathf.Sin(45 * Mathf.Deg2Rad);
//         //         float rotatedY = forward.y;
//         //         float rotatedZ = -forward.x * Mathf.Sin(45 * Mathf.Deg2Rad) + forward.z * Mathf.Cos(45 * Mathf.Deg2Rad);
//         float rotatedXRight = forward.x * Mathf.Cos(angle * Mathf.Deg2Rad) + forward.z * Mathf.Sin(angle * Mathf.Deg2Rad);
//         float rotatedYRight = forward.y;
//         float rotatedZRight = -forward.x * Mathf.Sin(angle * Mathf.Deg2Rad) + forward.z * Mathf.Cos(angle * Mathf.Deg2Rad);
// 
//         float rotatedXLeft = forward.x * Mathf.Cos(-angle * Mathf.Deg2Rad) + forward.z * Mathf.Sin(-angle * Mathf.Deg2Rad);
//         float rotatedYLeft = forward.y;
//         float rotatedZLeft = -forward.x * Mathf.Sin(-angle * Mathf.Deg2Rad) + forward.z * Mathf.Cos(-angle * Mathf.Deg2Rad);
// 
// 
//         Vector3 newRotatedVectorRight = new Vector3(rotatedXRight, rotatedYRight, rotatedZRight);
//         Vector3 newRotatedVectorLeft = new Vector3(rotatedXLeft, rotatedYLeft, rotatedZLeft);
// 
//         Debug.DrawLine
//             (
//                 transform.position,
//                 transform.position + newRotatedVectorRight * lineRange,
//                 Color.blue
//             );
// 
//         Debug.DrawLine
//             (
//                 transform.position,
//                 transform.position + newRotatedVectorLeft * lineRange,
//                 Color.blue
//             );
// 
// 
//         // Left
//         Debug.DrawLine
//             (
//                 transform.position,
//                 transform.position + transform.right * lineRange,
//                 Color.red
//             );
// 
//         // Right
//         Debug.DrawLine
//             (
//                 transform.position,
//                 transform.position + -transform.right * lineRange,
//                 Color.red
//             );
// 
//         // Forward
//         Debug.DrawLine
//             (
//                 transform.position,
//                 transform.position + transform.forward * lineRange,
//                 Color.red
//             );
// 
//         Debug.Log("Degrees " + viewConeAngle + " To Radians" + viewConeAngle * Mathf.Deg2Rad);
// 
//         // Lets Check If Player Is Close Enough To Us
//         Vector3 distanceAndDirectionToPlayer = playerTransform.position - transform.position;
//         if (distanceAndDirectionToPlayer.magnitude < visionDistance)
//         {
//             // We are inside the enemy's detection radius
//             Debug.Log("Player In Range");
//             colourChanger.SetState(EStateToColour.DefensiveYellow);
// 
//             // Now Lets Check Of They Are Withing Our "View Cone"
//             float dotProduct = Vector3.Dot(transform.forward, distanceAndDirectionToPlayer.normalized);
//             float arccosdot = Mathf.Acos(dotProduct);
//             // but why arccos? ust the equation nice simple example underneath
//             // https://www.wikihow.com/Find-the-Angle-Between-Two-Vectors
//             Debug.Log("Dot Product = " + dotProduct);
//             Debug.Log("View Angle In Radians = " + (viewConeAngle * Mathf.Deg2Rad));
//             Debug.Log("ArcCos = " + arccosdot);
//             if (arccosdot < (viewConeAngle * Mathf.Deg2Rad)) 
//             {
// 
//                 // They Are In Cone View! D:< Let's just double check we can even see them.
//                 float distanceToHeadset = distanceAndDirectionToPlayer.magnitude;
//                 Ray ray = new Ray(transform.position, distanceAndDirectionToPlayer.normalized);
//                 RaycastHit hitInfo;
//                 if (Physics.Raycast(ray, out hitInfo, distanceToHeadset))
//                 {
//                     if (hitInfo.collider.gameObject.name == "Player")
//                     {
//                         Debug.Log("Inside Code Range! Grrrrr");
//                         colourChanger.SetState(EStateToColour.AngryRed);
// 
//                         if (monsterBehaviour != null)
//                         {
//                             monsterBehaviour.OnPlayerSpotted(hitInfo.collider.gameObject.transform);
// 
//                             return;
//                         }
//                         //MonsterBehaviour.ChasePlayer(hitInfo.collider.gameObject);
//                     }
//                 }
//             }
//         }
//         else 
//         {
//             Debug.Log("Player Outside Range");
//             colourChanger.SetState(EStateToColour.IdleBlue);
//         }
// 
//         monsterBehaviour.OnLostSightOfPlayer();
// 
//         //RotateBehaviour();
// 
//     }
// 
//     
//     void RotateBehaviour()
//     {
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             Vector3 forward = transform.forward;
//             float angle = viewConeAngle;
//             //         float rotatedX = forward.x * Mathf.Cos(45 * Mathf.Deg2Rad) + forward.z * Mathf.Sin(45 * Mathf.Deg2Rad);
//             //         float rotatedY = forward.y;
//             //         float rotatedZ = -forward.x * Mathf.Sin(45 * Mathf.Deg2Rad) + forward.z * Mathf.Cos(45 * Mathf.Deg2Rad);
//             float rotatedX = forward.x * Mathf.Cos(angle * Mathf.Deg2Rad) + forward.z * Mathf.Sin(angle * Mathf.Deg2Rad);
//             float rotatedY = forward.y;
//             float rotatedZ = -forward.x * Mathf.Sin(angle * Mathf.Deg2Rad) + forward.z * Mathf.Cos(angle * Mathf.Deg2Rad);
// 
//             Vector3 newRotatedVectorRight = new Vector3(rotatedX, rotatedY, rotatedZ);
// 
//             transform.forward = newRotatedVectorRight;
// 
//         }
//     }
// 
// 
// 
// 
// 
// 
// }
