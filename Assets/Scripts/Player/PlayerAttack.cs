using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerAttack : MonoBehaviour
{
    [BoxGroup("Transform Reference")]
    [SerializeField] private Transform _attackPoint;
    [BoxGroup("Variables")]
    [SerializeField] private int _attackDamage = 30;
    [BoxGroup("Variables")]
    [SerializeField] private float _attackRate = 0f, _nextTimeAttack = 0f, _attackRange = 0.5f;
    [BoxGroup("Mask To Target")]
    [SerializeField] private LayerMask _enemyLayers;

    private Collider2D[] hitEnemies;
    public bool canAttack { get; private set; }

    private void Start()
    {
        canAttack = true;
    }

    private void FixedUpdate()
    {
        if(Time.time >= _nextTimeAttack)
        {
            canAttack = true;
            _nextTimeAttack = Time.time + 1f / _attackRate;
        }
    }

    public void OnPlayerAttack()
    {
        if (!canAttack) return;
        hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (!enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth)) return;
            enemyHealth.TakeDamage(_attackDamage);
        }
        canAttack = false;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
