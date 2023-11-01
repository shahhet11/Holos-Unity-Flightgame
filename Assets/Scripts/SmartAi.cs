using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAi : MonoBehaviour
{

    public float targetVelocity = 10.0f;
    public int numberOfRays = 17;
    public float angle = 90;
    public float rayRange = 2;
    // Start is called before the first frame update
    void Start()
    {
        //focusType
    }

    // Update is called once per frame
    void Update()
    {
        var deltapos = Vector3.zero;
        for (int i = 0; i < numberOfRays; ++i)
        {
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis(angle:(i/((float)numberOfRays - 1)) * angle * 2 - angle , this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward;

            var ray = new Ray(this.transform.position, direction);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, rayRange))
            {
                Debug.Log("OBSTACLE0");
                deltapos -= (1.0f / numberOfRays) * targetVelocity * direction;
            }
            else
            {
                Debug.Log("OBSTACLE!");
                deltapos += (1.0f / numberOfRays) * targetVelocity * direction;
            }
           



        }
        this.transform.position += deltapos * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < numberOfRays; ++i)
        {
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / ((float)numberOfRays - 1)) * angle * 2 - angle , this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward;
            Gizmos.DrawRay(this.transform.position, direction);
        }
    }
    }
