
using UnityEngine;
using System;

public class TempPlaneCpntrols : MonoBehaviour
{
    Transform _transform;
    Rigidbody _rigidbody;

    public Camera Camera;
    public Transform CameraTarget;
    [Range(0, 1)] public float CameraSpring = 0.96f;

    public float MinThrust = 600f;
    public float MaxThrust = 1200f;
    float _currentThrust;
    public float ThrustIncreaseSpeed = 400f;

    float _deltaPitch;
    public float PitchIncreaseSpeed = 300f;

    public float _deltaRoll;
    public float RollIncreaseSpeed = 300f;
    public Vector3 newDeltaRoll;

    public float Yaw;
    private float YawAmount = 100f;
    public float pitch;
    public float bank;
    public float roll = 0;
    public float speed = 0;
    public float verticalInput = 0;
    public float horizontalInput = 0;
    public float rollInput = 0;
    void Start()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();

        Camera.transform.SetParent(null);
    }

    void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        rollInput = Input.GetAxis("Roll");
        Yaw += horizontalInput * YawAmount * Time.deltaTime;
        pitch = Mathf.Lerp(0, 360, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        bank = Mathf.Lerp(0, 360, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);
        roll = Mathf.Lerp(0, 360, Mathf.Abs(rollInput)) * -Mathf.Sign(rollInput);

        newDeltaRoll = new Vector3(0, 0, roll) * Time.deltaTime;

        //transform.position = transform.forward * 50 * Time.deltaTime;
        // Y-axis = bank, X-axis = pitch and Z-axis = roll
        var thrustDelta = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            thrustDelta += ThrustIncreaseSpeed;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            thrustDelta -= ThrustIncreaseSpeed;
        }

        _currentThrust += thrustDelta * Time.deltaTime;
        _currentThrust = Mathf.Clamp(_currentThrust, MinThrust, MaxThrust);

        _deltaPitch = 0f;
        if (Input.GetKey(KeyCode.S))
        {
            _deltaPitch -= PitchIncreaseSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            _deltaPitch += PitchIncreaseSpeed;
        }

        _deltaPitch *= Time.deltaTime;

        //_deltaRoll = 0f;





        Quaternion yawRotation = Quaternion.Euler(0, Yaw, 0);
        Quaternion pitchRotation = Quaternion.Euler(pitch, 0, 0);
        Quaternion bankRotation = Quaternion.Euler(0, 0, bank);
        Quaternion rollRotation = Quaternion.Euler(newDeltaRoll);



        Debug.Log("rollRotation" + bankRotation.z);



        //Debug.Log("finalRotation" + finalRotation);

        if (Input.GetKey(KeyCode.Q))
        {
            Yaw = transform.localEulerAngles.y;
            float targetZRotationSpeed = roll + 5f;
            //float rollChangeSpeed = 5f;
            _deltaRoll = Mathf.Lerp(roll, targetZRotationSpeed, 2 * Time.deltaTime);
            if (roll >= 360)
            {
                roll = 0;
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            Yaw = transform.localEulerAngles.y;
            float targetZRotationSpeed = _deltaRoll - 20f;
            float rollChangeSpeed = 5f;
            _deltaRoll = Mathf.Lerp(_deltaRoll, targetZRotationSpeed, RollIncreaseSpeed * Time.deltaTime);
            if (roll >= 360)
            {
                roll = 0;
            }
        }
        //_deltaRoll *= Time.deltaTime * 2f;
        Quaternion finalRotation = Quaternion.Euler(Vector3.up * Yaw + Vector3.right * pitch + Vector3.forward * bank + Vector3.forward * roll);
        Quaternion finalRotationnew = yawRotation * pitchRotation * bankRotation * rollRotation;

        transform.localRotation = finalRotation;

        //var localRotation = _transform.localRotation;
        //localRotation *= Quaternion.Euler(0f, 0f, _deltaRoll);
        //transform.localRotation = localRotation;
    }

    void FixedUpdate()
    {



        var localRotation = _transform.localRotation;
        //localRotation *= Quaternion.Euler(0f, 0f, _deltaRoll);
        localRotation *= Quaternion.Euler(_deltaPitch, 0f, 0f);
        localRotation *= Quaternion.Euler(0f, _deltaRoll, 0);
        //_transform.localRotation = localRotation;
        _rigidbody.velocity = _transform.forward * (_currentThrust * Time.fixedDeltaTime);

        Vector3 cameraTargetPosition = _transform.position + _transform.forward * -8f + new Vector3(0f, 3f, 0f);
        var cameraTransform = Camera.transform;

        cameraTransform.position = cameraTransform.position * CameraSpring + cameraTargetPosition * (1 - CameraSpring);
        Camera.transform.LookAt(CameraTarget);
        speed = _rigidbody.velocity.magnitude;
    }
}