using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponManager : MonoBehaviour
{
    
    public int WeaponID;
    public Transform[] ShootPosition;
    public Transform MissileShootPosition;
    public GameObject Bullet;
    public GameObject Missile;
    public float speed = 1000;
    public float fireRate;
    private GameObject projectile;
    private float nextFire = 0.0f;
    Camera viewCamera;
    public EnemyTargetLock targetLock;
    public AmmoCustomization ammoCustomization;

    void Start()
    {

        viewCamera = Camera.main;


    }

    void Update()
    {

        
        if (Input.GetMouseButton(0) && Time.realtimeSinceStartup > nextFire)
        {
            //&& Time.realtimeSinceStartup > nextFire
            if (Bullet)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000f) && ShootPosition.Length > 0)
                {
                    nextFire = Time.realtimeSinceStartup + fireRate;
                    for (int i = 0; i < ShootPosition.Length; i++)
                    {
                        projectile = Instantiate(Bullet, ShootPosition[i].position, ShootPosition[i].rotation) as GameObject;
                        projectile.GetComponent<Rigidbody>().velocity = ShootPosition[i].forward * speed;
                    }
            }

            }
        }

        if (Input.GetMouseButtonDown(1) && Time.realtimeSinceStartup > nextFire + 1f)
        {
            if (targetLock.isTargeting && Missile)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000f))
                {
                    GameObject projectile = Instantiate(Missile, MissileShootPosition.position, MissileShootPosition.rotation) as GameObject;
                    MissileController missileController = projectile.GetComponent<MissileController>();
                    missileController.SetTarget(targetLock.currentTarget); 
                    missileController.SetSpeed(80);
                }
                ammoCustomization.InterpolateAmmoColor(60);
                ammoCustomization.AmmoUpdate();

                ammoCustomization.AmmoIconLayer0[0].SetActive(true);
                ammoCustomization.AmmoIconLayer0[1].SetActive(true);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ammoCustomization.AmmoIconLayer0[0].SetActive(false);
            ammoCustomization.AmmoIconLayer0[1].SetActive(false);
        }

    }


}
