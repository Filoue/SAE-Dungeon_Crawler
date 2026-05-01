using System;
using System.Collections;
using UnityEngine;

enum ArcherState
{
    Idle, 
    Approching, 
    Attacking, 
    Retreating
}
[RequireComponent(typeof(EnemyHealth))]
public class ArcherAI : MonoBehaviour
{
    [Header("Distance")] 
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float keepDistance = 4f;
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Combat")] 
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1.5f;
    private float nextFireTime;

    private Transform player;
    [SerializeField] private Animator anim;
    private ArcherState currentState;

    private EnemyHealth _enemyHealth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        firePoint = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < keepDistance)
        {
            currentState = ArcherState.Retreating;
        } else if (distanceToPlayer <= attackRange)
        {
            currentState = ArcherState.Attacking;
        }
        else
        {
            currentState = ArcherState.Approching;
        }

        ExecuteState(distanceToPlayer);
    }

    private void ExecuteState(float distance)
    {
        switch (currentState)
        {
            case ArcherState.Approching:
                Move(player.position, moveSpeed);
                anim.SetBool("Attack", false);
                break;
            case ArcherState.Attacking:
                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                }
                break;
            case ArcherState.Retreating:
                Vector2 fleePos = (Vector2)transform.position +
                                  ((Vector2)transform.position - (Vector2)player.position).normalized;
                Move(fleePos, moveSpeed * 0.8f); // Fuit lentement donc oui magic number
                anim.SetBool("Attack", false);
                break;
        }
    }

    private void Shoot()
    {
        anim.SetBool("Attack", true);
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

        Invoke("ResetAttackAnim", 0.3f);
    }

    void ResetAttackAnim() => anim.SetBool("Attack", false);
    

    private void Move(Vector2 target, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        Vector2 dir = player.position - transform.position;
        GetComponentInChildren<SpriteRenderer>().flipX = dir.x < 0;
    }
}
