using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = -1;

    // Observer - eventos que notifican a los observadores
    public UnityEvent OnDeath;
    public UnityEvent<int> OnHPChanged;

    void Awake()
    {
        if (currentHealth < 0) currentHealth = maxHealth;
        NotifyHPChanged();
    }

    // M�todo para el obverver
    private void NotifyHPChanged()
    {
        OnHPChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        NotifyHPChanged();

        // Observer - notifica a los observadores sobre la muerte
        if (currentHealth == 0)
            OnDeath?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        NotifyHPChanged();
    }

    public void SetHP(int value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        NotifyHPChanged();
        if (currentHealth == 0) OnDeath?.Invoke();
    }

    public void SetMaxHealth(int newMax, bool keepPercentage = true)
    {
        if (newMax <= 0) newMax = 1;

        if (keepPercentage)
        {
            float pct = (float)currentHealth / Mathf.Max(1, maxHealth);
            maxHealth = newMax;
            currentHealth = Mathf.Clamp(Mathf.RoundToInt(pct * maxHealth), 0, maxHealth);
        }
        else
        {
            maxHealth = newMax;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        NotifyHPChanged();
    }

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
}
