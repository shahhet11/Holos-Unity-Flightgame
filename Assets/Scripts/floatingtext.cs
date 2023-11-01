using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingtext : MonoBehaviour
{

    public Vector3 Offset = new Vector3(0,2,0);
    public Vector3 RandomizeIntensity = new Vector3(0.5f,0,0);
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject)
        {
            Destroy(gameObject, 3f);
            transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
        }
    }

}
