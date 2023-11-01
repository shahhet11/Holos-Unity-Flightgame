using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    public int attackDamage;
    public float attackTime;
    public bool playerInRange;

    private float timerT;
    private PlaneHealth PlaneHealth;
    private Vector3 playerpos;

    void Start()
    {
        attackTime = GameManager.Instance.timeBetweenAttackAI;
        PlaneHealth = GameManager.Instance.PlaneHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerpos = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        timerT += Time.unscaledDeltaTime;

        if (timerT >= attackTime && playerInRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        timerT = 0f;

        if (PlaneHealth.HealthPoints > 0)
            PlaneHealth.UpdateHealth(attackDamage, playerpos);
    }
}
