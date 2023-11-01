using UnityEngine;

public class AIPlaneController : MonoBehaviour
{
    Transform _transform;
    Rigidbody _rigidbody;

    public Transform playerPlane;
    public LayerMask obstacleLayer; 

    public float MinThrust = 600f;
    public float MaxThrust = 1200f;
    public float ThrustIncreaseSpeed = 400f;
    public float PitchControlSpeed = 2.0f;
    public float RollControlSpeed = 2.0f;
    public float FollowDistance = 100.0f;
    public float AvoidanceDistance = 50.0f; 
    public float PlaneSpeed = 0;
    void Start()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        if (playerPlane == null)
        {
            playerPlane = FindObjectOfType<PlaneController>().transform;
         
        }
    }

    void FixedUpdate()
    {
        if (playerPlane == null)
        {
            playerPlane = FindObjectOfType<PlaneController>().transform;
            return;
        }

        Vector3 toPlayer = playerPlane.position - _transform.position;

        float pitchInput = Mathf.Clamp(-Vector3.Angle(_transform.forward, toPlayer) / FollowDistance, -1, 1);
        float rollInput = Mathf.Clamp(Vector3.SignedAngle(_transform.forward, toPlayer, _transform.up) / FollowDistance, -1, 1);

        float desiredThrust = playerPlane.GetComponent<Rigidbody>().velocity.magnitude;
        float thrustInput = (desiredThrust - _rigidbody.velocity.magnitude) / ThrustIncreaseSpeed;

        RaycastHit hit;
        if (Physics.Raycast(_transform.position, _transform.forward, out hit, AvoidanceDistance, obstacleLayer))
        {
            Debug.Log("obstacleLayer"+ obstacleLayer);
            pitchInput = Mathf.Clamp(-hit.normal.z, -1, 1);
            rollInput = Mathf.Clamp(hit.normal.x, -1, 1);
        }
        //Debug.Log("pitchInput"+ pitchInput);
        //Debug.Log("rollInput"+ rollInput);
        // Apply pitch, roll, and thrust adjustments
        _transform.Rotate(pitchInput * PitchControlSpeed, 0, -rollInput * RollControlSpeed);
        float targetThrust = Mathf.Clamp(_rigidbody.velocity.magnitude + thrustInput * ThrustIncreaseSpeed * Time.fixedDeltaTime, MinThrust, MaxThrust);
        _rigidbody.velocity = _transform.forward * targetThrust * Time.fixedDeltaTime;
        PlaneSpeed = _rigidbody.velocity.magnitude;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            _rigidbody.isKinematic = true;
            _rigidbody.isKinematic = false;
            Debug.Log("Collided with an obstacle.");
        }
    }
}