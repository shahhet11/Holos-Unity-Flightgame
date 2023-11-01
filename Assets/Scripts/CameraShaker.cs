
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public Transform Camera;
    public float Intensity = 1f;
    [Range(0.1f, 1f)]
    public float Duration = 1f;

    private Vector3 _offset;
    public float _amount;
    private float _current;
    public float shakeDuration = 0f;
    public Vector3 originalPosition;
    private Transform cameraTransform;
    public float decreaseFactor = 1.0f;
    public float timetakenfordelay = 0;
    private void Awake()
    {
        cameraTransform = GetComponent<Transform>();
    }
    public void Start()
    {
        originalPosition = cameraTransform.localPosition;
        _current = Duration;
    }

    public void Shake()
    {
        _current = 0f;
    }

    void Update()
    {
       
        if(timetakenfordelay <= 3.2f)
        {
            timetakenfordelay += Time.deltaTime;
            if(timetakenfordelay >= 3f)
            {
                //originalPosition = cameraTransform.localPosition;
            }
        }
        //if (shakeDuration > 0)
        //{
            
        //    _current += Time.deltaTime;
        //    _amount = 1f - Mathf.Clamp01(_current / Duration);

        //    //if (_amount <= 0)
        //    //{
        //    //    Debug.Log("WENTINS" + _amount);
        //    //    Camera.localPosition = Vector3.zero;
        //    //    return;
        //    //}
        //    //cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
        //    shakeDuration -= Time.deltaTime * decreaseFactor;
        //    _offset = new Vector3(
        //    UnityEngine.Random.Range(-1f, 1f),
        //    UnityEngine.Random.Range(-1f, 1f),
        //    UnityEngine.Random.Range(-1f, 1f)
        //);
           
        //    Camera.localPosition = _offset * _amount * Intensity;
        //}
        //else
        //{
        //    //shakeDuration = 0f;
        //    //cameraTransform.localPosition = originalPosition;
            
        //}
        if (shakeDuration > 0)
        {
            _offset = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f)
        );

            //Camera.localPosition = _offset * _amount * Intensity;
            cameraTransform.localPosition = cameraTransform.localPosition + Random.insideUnitSphere * 0.7f;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            
            //cameraTransform.localPosition = originalPosition;
            
        }


    }
}