
using UnityEngine;

public class MissileController : MonoBehaviour
{
    private Transform target;
    private float speed;
    public Vector3 DirectionX;
    public SinusoidalWave s_Wave;
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        s_Wave = GetComponentInChildren<SinusoidalWave>();

        if (s_Wave != null)
        {

        }
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            GetComponent<Rigidbody>().velocity = direction * speed;
            DirectionX += transform.position * Time.deltaTime * speed;
            if(s_Wave) {
                Vector3 Direction = transform.forward; 
                s_Wave.pos = transform.position + Direction * s_Wave.movespeed * Time.deltaTime;

                s_Wave.transform.position = s_Wave.pos + transform.up * Mathf.Sin(Time.time * s_Wave.frequency) * s_Wave.Magnitude;

                //s_Wave.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
            }
        }
    }
}




