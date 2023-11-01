
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCustomization : MonoBehaviour
{

    public GameObject[] AmmoIconLayer0;
    public Text ammoMagsTxt;
    public Text ammoRoundsTxt;
    public GameObject[] Ammos;
    private Coroutine currentCoroutine;
    public float transitionDuration = 1.0f;
    public float fadeDuration = 1.0f;
    private Color initialColor;
    public Color targetColor;
    [System.Serializable]
    public class AmmoObject
    {
        public string WeaponName;
        public int WeaponID;
        public int ammoCurrent;
        public int ammoRounds;
        public int ammoMags;
        public bool isReloading;
    }

    public List<AmmoObject> weaponAmmoList = new List<AmmoObject>();


    private void Start()
    {
        initialColor = ammoRoundsTxt.color;
    }
    public void Reload()
    {
        int bulletsToReload = weaponAmmoList[0].ammoRounds - weaponAmmoList[0].ammoCurrent;
        weaponAmmoList[0].isReloading = true;

        if (bulletsToReload <= weaponAmmoList[0].ammoMags)
        {
            StartCoroutine(LerpAmmo(weaponAmmoList[0].ammoCurrent + bulletsToReload, weaponAmmoList[0].ammoMags - bulletsToReload));
            weaponAmmoList[0].ammoCurrent += bulletsToReload;
            weaponAmmoList[0].ammoMags -= bulletsToReload;
        }
        else
        {
            StartCoroutine(LerpAmmo(weaponAmmoList[0].ammoCurrent + weaponAmmoList[0].ammoMags, 0));
            weaponAmmoList[0].ammoCurrent += weaponAmmoList[0].ammoMags;
            weaponAmmoList[0].ammoMags = 0;

        }
        currentCoroutine = StartCoroutine(ActivateAmmosWithDelay(weaponAmmoList[0].ammoCurrent));
        ammoRoundsTxt.text = weaponAmmoList[0].ammoCurrent.ToString();
        ammoMagsTxt.text = weaponAmmoList[0].ammoMags.ToString();
    }
    private IEnumerator LerpAmmo(int targetCurrentAmmo, int targetTotalAmmo)
    {
        float duration = 1.0f; 
        float startTime = Time.time;
        int startCurrentAmmo = weaponAmmoList[0].ammoCurrent;
        int startTotalAmmo = weaponAmmoList[0].ammoMags;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            weaponAmmoList[0].ammoCurrent = Mathf.RoundToInt(Mathf.Lerp(startCurrentAmmo, targetCurrentAmmo, t));
            weaponAmmoList[0].ammoMags = Mathf.RoundToInt(Mathf.Lerp(startTotalAmmo, targetTotalAmmo, t));

            ammoRoundsTxt.text = weaponAmmoList[0].ammoCurrent.ToString("00");
            ammoMagsTxt.text = weaponAmmoList[0].ammoMags.ToString("00");


            yield return null;
        }

        weaponAmmoList[0].ammoCurrent = targetCurrentAmmo;
        weaponAmmoList[0].ammoMags = targetTotalAmmo;


        ammoRoundsTxt.text = weaponAmmoList[0].ammoCurrent.ToString("00");
        ammoMagsTxt.text = weaponAmmoList[0].ammoMags.ToString("00");
        weaponAmmoList[0].isReloading = false;
    }
    public IEnumerator ActivateAmmosWithDelay(int currentAmmo)
    {
        float activationDelay = 0.05f;
        for (int i = 0; i < weaponAmmoList[0].ammoRounds; i++)
        {
            Ammos[i].SetActive(false);
        }
        for (int i = 0; i < currentAmmo; i++)
        {
            Ammos[i].SetActive(true);
            yield return new WaitForSeconds(activationDelay);
        }
    }
    public void InterpolateAmmoColor(int alpha)
    {
        StartCoroutine(FadeInOut());
    }
    private IEnumerator FadeInOut()
    {
        float elapsedTime = 0f;
        Color targetColor = ammoRoundsTxt.color;
        targetColor.a = 0; 

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            ammoRoundsTxt.color = Color.Lerp(initialColor, targetColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ammoRoundsTxt.color = targetColor;

        yield return new WaitForSeconds(0.2f);
        targetColor = initialColor;
        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            ammoRoundsTxt.color = Color.Lerp(targetColor, initialColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ammoRoundsTxt.color = initialColor;
    }

    public void ShootAmmo()
    {
        if (weaponAmmoList[0].ammoCurrent > 0)
        {
            weaponAmmoList[0].ammoCurrent--;
            ammoRoundsTxt.text = weaponAmmoList[0].ammoCurrent.ToString("00");
            ammoMagsTxt.text = weaponAmmoList[0].ammoMags.ToString("00");
            Ammos[weaponAmmoList[0].ammoCurrent].SetActive(false);
            for (int i = weaponAmmoList[0].ammoCurrent; i < 20; i++)
            {
                if (Ammos[i].activeInHierarchy)
                {
                    Ammos[i].SetActive(false);
                }
            }
        }
    }
    public void AmmoUpdate()
    {
        if (weaponAmmoList[0].ammoCurrent > 0)
        {
            ShootAmmo();
        }
        if (weaponAmmoList[0].ammoCurrent == 0 && weaponAmmoList[0].ammoMags > 0)
        {
            Reload();
        }
        else
        {
            
        }
    }




}
