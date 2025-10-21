using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private Health health;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool useAutoAttack = false;

    [Header("State Machine")]
    public EnemyStateMachine stateMachine;

    private float cooldown;
    private bool canAttack = true;

    void Awake()
    {
        if (health == null) health = GetComponent<Health>();
        if (stateMachine == null) stateMachine = GetComponent<EnemyStateMachine>();
        stateMachine?.Initialize(this);
    }

    void OnEnable()
    {
        if (health != null) health.OnDeath.AddListener(HandleDeath);
    }

    void OnDisable()
    {
        if (health != null) health.OnDeath.RemoveListener(HandleDeath);
    }

    void Start()
    {
        cooldown = attackCooldown;
        canAttack = (health == null) ? true : (health.CurrentHealth > 0);
        stateMachine?.ChangeState(EnemyStateMachine.EnemyState.Idle);
    }

    void Update()
    {
        cooldown -= Time.deltaTime;
        stateMachine?.UpdateState();

        if (!useAutoAttack || !canAttack) return;

        if (cooldown <= 0f)
        {
            AttackNow();
            cooldown = attackCooldown;
        }
    }

    public void AttackNow()
    {
        if (!canAttack) return;
        if (Player.Instance != null)
            Player.Instance.TakeDamage(damage);
    }

    public void TakeDamage(int dmg)
    {
        if (health == null || dmg <= 0) return;
        health.TakeDamage(dmg);
        stateMachine?.Hurt();
    }

    public void Heal(int amount)
    {
        health?.Heal(amount);
    }

    public void Slow(float extraCooldown)
    {
        cooldown += extraCooldown;
        if (cooldown < 0.1f) cooldown = 0.1f;
    }

    public void Slow(float magnitude, float duration)
    {
        cooldown += magnitude;
        StartCoroutine(RemoveSlowAfter(duration, magnitude));
    }

    private IEnumerator RemoveSlowAfter(float duration, float magnitude)
    {
        yield return new WaitForSeconds(duration);
        cooldown -= magnitude;
        if (cooldown < 0.1f) cooldown = 0.1f;
    }

    private void HandleDeath()
    {
        canAttack = false;
        StopAllCoroutines();
        CancelInvoke();

        var col2d = GetComponent<Collider2D>();
        if (col2d) col2d.enabled = false;

        useAutoAttack = false;
        stateMachine?.ChangeState(EnemyStateMachine.EnemyState.Dead);
    }
}
