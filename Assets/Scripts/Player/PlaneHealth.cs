using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlaneHealth : MonoBehaviour
{
    public GameObject blastPrefab;

    public float HealthPoints;
    public Image healthSlider;
    public Text healthText;

    [Header("Damage")]
    public int dmg_bullet;
    public static PlaneHealth ph_instance;
    public bool shieldON;
    public bool isHit;
    public CameraShaker cameraShake;

    void Start()
    {
        ph_instance = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Enemy"))
        {
            if (cameraShake != null)
            {
                cameraShake.shakeDuration = 0.5f; // Adjust the duration as needed
            }

            Destroy(collision.gameObject);
            int id = collision.gameObject.GetComponent<Damage>().dmgId;
            dmg_bullet = GameManager.Instance.DamageList[0];
            UpdateHealth(dmg_bullet, collision.transform.position);
            //isHit = true;

        }
    }

    public void UpdateHealth(int Damage, Vector3 pos)
    {
        float calculate = (Damage * HealthPoints) / 100;
        HealthPoints -= calculate;
        GameManager.Instance.CameraShaker.Shake();
        GameManager.Instance.PlaneHealth.SetHealthSlider(HealthPoints);

        if (HealthPoints <= 1)
        {
            GameManager.Instance.PlayerDeath();
            GameManager.Instance.CameraShaker.Intensity = 1.5f;
            GameManager.Instance.CameraShaker.Shake();

            GameObject blast = Instantiate(blastPrefab, pos, Quaternion.identity);
            Destroy(blast, 1f);

            gameObject.SetActive(false);
        }
    }

    public void SetHealthSlider(float hp)
    {
        int maxHealth = GameManager.Instance.maxHealthPlayer;
        float total = ((float)(hp * 1f) / 100);
        if (total < 0f)
            total = 0f;

        healthSlider.fillAmount = total;

        healthText.text = HealthPoints.ToString("00");
    }
}
