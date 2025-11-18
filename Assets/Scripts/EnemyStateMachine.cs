using UnityEngine;
using System.Collections;

//State - gestiona los estados del enemigo
//Strategy - cada estado representa una estrategia de comportamiento diferente
public class EnemyStateMachine : MonoBehaviour
{
    //State - Define los posibles estados
    public enum EnemyState
    {
        Idle,
        Attacking,
        Hurt,
        Dead
    }

    //State - estado actual del enemigo
    private EnemyState currentState;
    private Enemy enemy;

    public void Initialize(Enemy e)
    {
        enemy = e;
        currentState = EnemyState.Idle;
    }

    //State - cambia el estado del enemigo
    //Strategy - al cambiar el estado, cambia la estrategia de comportamiento
    public void ChangeState(EnemyState newState)
    {
        currentState = newState;

        //Strategy - cada caso representa una estrategia diferente de comportamiento
        switch (currentState)
        {
            case EnemyState.Idle: // Espera/pasividad
                break;
            case EnemyState.Attacking: // Ataque agresivo
                break;
            case EnemyState.Hurt: // Reacción al daño
                StartCoroutine(HurtCoroutine());
                break;
            case EnemyState.Dead: // Inactividad permanente
                break;
        }
    }

    //State - actualiza el comportamiento según el estado actual
    //Strategy - el comportamiento se ejecuta según la estrategia (estado) actual
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
