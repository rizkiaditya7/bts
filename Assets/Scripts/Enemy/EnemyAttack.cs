using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyAttack : MonoBehaviour
{
    [BoxGroup("Event")]
    [SerializeField] GameEventNoParam _onEnemyAttackPlayerEvent;
    [BoxGroup("Damage Value")]
    [SerializeField] private int _damage = 10;
    [BoxGroup("Cooldown To Next Attack")]
    [SerializeField] private float _attackCooldown = 3f;
    private float _timeSinceLastAttack;
    private bool _canAttack;
    private GameObject _playerTarget;

    private void Start()
    {
        _canAttack = true;
    }

    private void Update()
    {
        CooldownHandler();
    }

    private void CooldownHandler()
    {
        _timeSinceLastAttack += Time.deltaTime;
        if (_playerTarget == null) return;
        float distance = Vector2.Distance(transform.position, _playerTarget.transform.position);
        if (distance > 2) return;
        if (_timeSinceLastAttack >= _attackCooldown)
        {
            _canAttack = true;
            _timeSinceLastAttack = 0f;
            OnEnemyAttack();
        }
    }

    public void OnEnemyAttack()
    {
        if (!_canAttack) return;
        _canAttack = false;
        if (!_playerTarget.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth)) return;
        playerHealth.TakeDamage(_damage);
        _onEnemyAttackPlayerEvent.Raise();

    }

    public void GetTargetPlayer(Component sender, object data)
    {
        if (data is GameObject)
        {
            _playerTarget = (GameObject)data;
        }
    }
}
