using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShoot : MonoBehaviour
{
    public GameObject pref;
    public Transform[] shootPosition;

    public float speed = 2500f;
    public float fireRate;
    public float fromRate;
    public float toRate;

    private float timer;
    private float tempRate;

    // Start is called before the first frame update
    void Start()
    {
        fireRate = Random.Range(fromRate, toRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= fireRate)
        {
            Shoot();

            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    private void Shoot()
    {
        for (int i = 0; i < shootPosition.Length; i++)
        {
            GameObject projectile;
            projectile = Instantiate(pref, shootPosition[i].position, shootPosition[i].rotation) as GameObject;
            projectile.GetComponent<Rigidbody>().AddForce(shootPosition[i].forward * speed);
        }
    }
}
