using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalWave : MonoBehaviour
{

    public float movespeed = 5f;// Horizontal Speed
    public float frequency = 20f;// How often it goes up and down
    public float Magnitude = 0.5f;// implies the diff between upper and lower points

    bool facingRight = true;
    public Vector3 pos , localScale; // Position Of Object

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
    }

    void CheckWheretoFace()
    {
        // not need right now
    }

    public void Movement()
    {
        pos += transform.right * Time.deltaTime * movespeed;
        Debug.Log("s_Wave"+pos);
        transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * Magnitude;
    }

}
