using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotationAxis;

    protected void Update()
    {
        transform.Rotate(rotationAxis * Time.deltaTime);
    }
}
