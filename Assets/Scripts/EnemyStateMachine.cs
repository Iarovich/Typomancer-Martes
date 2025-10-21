using UnityEngine;
using System.Collections;

public class EnemyStateMachine : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Attacking,
        Hurt,
        Dead
    }

    private EnemyState currentState;
    private Enemy enemy;

    public void Initialize(Enemy e)
    {
        enemy = e;
        currentState = EnemyState.Idle;
    }

    public void ChangeState(EnemyState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Attacking:
                break;
            case EnemyState.Hurt:
                StartCoroutine(HurtCoroutine());
                break;
            case EnemyState.Dead:
                break;
        }
    }

    public void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Attacking:
                break;
            case EnemyState.Hurt:
                break;
            case EnemyState.Dead:
                break;
        }
    }

    public void Hurt()
    {
        if (currentState != EnemyState.Dead)
            ChangeState(EnemyState.Hurt);
    }

    public void Slow(float extraCooldown)
    {
        enemy?.Slow(extraCooldown);
    }

    private IEnumerator HurtCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (enemy != null && currentState != EnemyState.Dead)
            ChangeState(EnemyState.Idle);
    }
}
