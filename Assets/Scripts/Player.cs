using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    //Singleton
    public static Player Instance { get; private set; }

    private Health health;

    [Header("Convenience refs (opcional)")]
    public DeckSystem deckSystem;     
    public HandManager handManager;   
    public SpellHistory spellHistory; 

    void Awake()
    {
        //Singleton - garantiza una unica instancia global
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Ya existe una instancia de Player, eliminando esta.");
            Destroy(this);
            return;
        }
        Instance = this;

        health = GetComponent<Health>();
        if (health == null)
            Debug.LogError("Player necesita un componente Health en el mismo GameObject.");
    }

   
    public void TakeDamage(int amount)
    {
        health?.TakeDamage(amount);
    }

    public void Heal(int amount)
    {
        health?.Heal(amount);
    }

    public int CurrentHealth => health != null ? health.CurrentHealth : 0;
    public int MaxHealth => health != null ? health.MaxHealth : 0;
    public Health PlayerHealth => health;

    
    public void DrawInitialHand()
    {
        if (deckSystem != null && handManager != null)
        {
 
            handManager.StartHand();
        }
    }

    public void AddCardToDeck(CardData card)
    {
        if (deckSystem != null) deckSystem.AddCardToDeck(card);
    }
}
