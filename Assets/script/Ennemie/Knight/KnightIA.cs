using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyState
{
    Chasing,
    Attacking,
    Fleeing,
    Resting,
    Enraged
}
[RequireComponent(typeof(EnemyHealth))]
public class KnightIA : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 3f;
    public float enragedSpeed = 5f;
    public float attackRange = 1.5f;
    public float fleeingSpeed = 1.5f;
    public float restingTime = 2f;


    [Header("Refs")]
    public Transform player;
    [SerializeField] private Animator anim;
    private EnemyState currentState;
    private bool isEnraged = false;
    private EnemyHealth _enemyHealth;

    void Start()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        currentState = EnemyState.Chasing;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        CheckHealth();
        switch (currentState) {
            case EnemyState.Chasing: MoveTowardsPlayer(isEnraged ? enragedSpeed : speed); break;
            case EnemyState.Attacking: /* Géré par les triggers ou la distance */ break;
            case EnemyState.Fleeing: FleeFromPlayer(); break;
            case EnemyState.Resting: /* Attend la fin de la coroutine */ break;
            case EnemyState.Enraged: MoveTowardsPlayer(enragedSpeed); break;
        }
        Vector2 dir = player.position - transform.position;
        GetComponentInChildren<SpriteRenderer>().flipX = dir.x < 0;
    }

    void MoveTowardsPlayer(float currentSpeed) {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange) {
            anim.SetBool("Attack1", false);
            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        } else {
            Attack();
        }
    }

    void Attack() {
        currentState = EnemyState.Attacking;
        anim.SetBool("Attack1", true);
        
        Invoke("ResetAttack", 0.5f); 
    }

    void ResetAttack() {
        if(currentState != EnemyState.Fleeing) currentState = EnemyState.Chasing;
    }

    void CheckHealth() {
        if (_enemyHealth.currentHealth < _enemyHealth.maxHealth * 0.2f && !isEnraged && currentState != EnemyState.Resting && currentState != EnemyState.Fleeing) {
            StartCoroutine(FleeAndRecover());
        }
    }

    IEnumerator FleeAndRecover() {
        currentState = EnemyState.Fleeing;
        yield return new WaitForSeconds(fleeingSpeed);

        currentState = EnemyState.Resting;
        yield return new WaitForSeconds(restingTime); 

        isEnraged = true;
        currentState = EnemyState.Enraged;
        // Optionnel : accélérer l'animation d'attaque ici
        anim.speed = 1.5f; 
    }

    void FleeFromPlayer() {
        anim.SetBool("Attack1", false);
        
        Vector2 direction = (transform.position - player.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }
}
