using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLookAt : MonoBehaviour
{
    public static WeaponLookAt weaponLook;
    public Transform[] PlayerGuns;
    private void Awake()
    {
        //Invoke("CallDelay", 0.9999f);
    }

    void CallDelay()
    {
        this.enabled = true;
    }

    private void OnEnable()
    {

        //weaponLook = this;
    }

    private void FixedUpdate()
    {
        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (Physics.Raycast(rayOrigin, out hitinfo))
        {
            
                Vector3 direction = hitinfo.point - PlayerGuns[0].position;
                PlayerGuns[0].rotation = Quaternion.LookRotation(direction);
                PlayerGuns[1].rotation = Quaternion.LookRotation(direction);

        }
    }
}
