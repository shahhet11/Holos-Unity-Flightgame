using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    public GameObject blastPrefab;
    public GameObject healthBox;
    public GameObject weaponBox;
    public GameObject FloatingTextPrefab;

    public int HealthPoints;

    [Header("Damage")]
    public int dmg_bullet_percent;

    void Start()
    {
        HealthPoints = GameManager.Instance.maxHealthAI;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);

            int id = collision.gameObject.GetComponent<Damage>().dmgId;
            dmg_bullet_percent = GameManager.Instance.DamageList[id];

            UpdateHealth(dmg_bullet_percent, collision.transform.position);
        }
    }
    void ShowFloatingText(int damage)
    {
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        float random = Random.Range(-2, 3);

        go.GetComponent<TextMesh>().offsetZ = random;
        go.GetComponent<TextMesh>().text = damage.ToString();
    }
    public void UpdateHealth(int Damage, Vector3 pos)
    {
        HealthPoints -= Damage;
        if (FloatingTextPrefab)
        {
            ShowFloatingText(Damage);
        }
        GameManager.Instance.CameraShaker.Shake();
        if (HealthPoints <= 0)
        {
            GameManager.Instance.killCounter++;

            GameManager.Instance.CameraShaker.Shake();
            GameObject blast = Instantiate(blastPrefab, pos, Quaternion.identity);
            Destroy(blast, 1f);
            //GameManager.Instance.CameraShaker.Shake();
            Destroy(gameObject);
        }
    }
}
