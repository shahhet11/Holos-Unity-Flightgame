
using UnityEngine;
using System;
using TMPro;

public class PlaneController : MonoBehaviour
{
    Transform _transform;
    Rigidbody _rigidbody;

    public Camera Camera;
    public Transform CameraTarget;
    [Range(0, 1)] public float CameraSpring = 0.96f;
    bool isFixed = false;
    public float MinThrust = 600f;
    public float MaxThrust = 1200f;
    public float currentThrust;
    public float ThrustIncreaseSpeed = 400f;
    public float PitchIncreaseSpeed = 300f;
    public float RollIncreaseSpeed = 300f;



    public float Yaw;
    private float YawAmount = 100f;
    public float pitch;
    public float bank;
    public float roll = 0;
    public float speed = 0;
    public float verticalInput = 0;
    public float horizontalInput = 0;
    public float rollInput = 0;
    public bool isHoldingCtrl;
    //JET VFX and SFX
    public AudioSource audioSource;
    public float targetVolume = 0.2f; 
    public float fadeDuration = 5.0f;  

    private float initialVolume;
    private float timer = 0.0f;

    public bool isJetPackFlameOn = false;
    public AudioSource JetSfx;
    public AudioClip[] JetTransitionClips;
    public bool JetPackAllowed = true;
    public ParticleSystem[] JetParticles;
    public float timeLeft = 30f;

    public UnityEngine.UI.Slider JetPackSlider;
    void Start()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        JetAudioInit();
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        rollInput = Input.GetAxis("Roll");
        Yaw += horizontalInput * YawAmount * Time.deltaTime;
        pitch = Mathf.Lerp(pitch, pitch + .5f * verticalInput, Mathf.Abs(verticalInput));
        bank = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);
        roll = Mathf.Lerp(roll, roll + 2f * rollInput, Mathf.Abs(rollInput));


        // Y-axis = bank, X-axis = pitch and Z-axis = roll
        var thrustDelta = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            thrustDelta += ThrustIncreaseSpeed;
            if(timeLeft < 0)
            {
                JetPackAllowed = false;
            }
            if (JetPackAllowed)
            {
                JetBoosterVFX();
                timeLeft -= Time.deltaTime;
                JetPackSlider.value = timeLeft / 30;
            }
            else
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0)
                {
                    JetPackAllowed = true;
                    timeLeft = 30;
                }
            }
        }
        else
        {
            if (isJetPackFlameOn)
            {
                for (int i = 0; i < JetParticles.Length; i++)
                {
                    JetParticles[i].Stop();
                }
                JetStopSound();
                isJetPackFlameOn = false;
            }
            if (timeLeft <= 29.9f)
            {
                timeLeft += Time.deltaTime;
                JetPackSlider.value = timeLeft / 30;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            thrustDelta -= ThrustIncreaseSpeed;
        }

        currentThrust += thrustDelta * Time.deltaTime;
        currentThrust = Mathf.Clamp(currentThrust, MinThrust, MaxThrust);

        Quaternion finalRotation = Quaternion.Euler(Vector3.right * pitch + Vector3.up * Yaw  +Vector3.forward * bank + Vector3.forward * roll);

        _transform.localRotation = finalRotation;
    }

    void FixedUpdate()
    {

        _rigidbody.velocity = _transform.forward * (currentThrust * Time.fixedDeltaTime);

        Vector3 cameraTargetPosition = _transform.position + _transform.forward * -8f + new Vector3(0f, 3f, 0f);
        var cameraTransform = Camera.transform;

        cameraTransform.position = cameraTransform.position * CameraSpring + cameraTargetPosition * (1 - CameraSpring);
        Camera.transform.LookAt(CameraTarget);
        speed = _rigidbody.velocity.magnitude;
    }
   
    private void JetAudioInit()
    {
        if (audioSource.volume < targetVolume)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            audioSource.volume = Mathf.Lerp(initialVolume, targetVolume, t);
        }
    }

    private void JetBoosterVFX()
    {
        if (JetPackAllowed)
        {
            if (!isJetPackFlameOn)
            {

                for (int i = 0; i < JetParticles.Length; i++)
                {
                    JetParticles[i].Play();
                }
                JetStartSound();

                isJetPackFlameOn = true;
            }
        }
    }

    void JetStartSound()
    {
        if (JetSfx.loop)
            JetSfx.loop = false;
        JetSfx.clip = JetTransitionClips[0];
        JetSfx.Play();
        Invoke("JetFlameContinuity", 0.9f);
    }
    void JetFlameContinuity()
    {
        JetSfx.clip = JetTransitionClips[2];
        JetSfx.loop = true;
        JetSfx.Play();
    }
    void JetStopSound()
    {
        if (JetSfx.loop)
            JetSfx.loop = false;
        CancelInvoke("JetFlameContinuity");
        JetSfx.clip = JetTransitionClips[1];
        JetSfx.Play();
    }
}