
using System.Collections.Generic;
using UnityEngine;

public class DeckSystem : MonoBehaviour
{
    [Header("Cartas base por elemento (distribución fija)")]
    public CardData chaosCard; // 10
    public CardData lightCard; // 10
    public CardData darkCard;  // 5
    public CardData waterCard; // 15
    public CardData fireCard;  // 15
    public CardData airCard;   // 10

    private readonly Queue<CardData> drawPile = new Queue<CardData>(); // mazo principal
    private readonly Stack<CardData> discardPile = new Stack<CardData>(); // mazo de descarte

    private System.Random rng = new System.Random();

    void Awake()
    {
        InitializeDeck();
    }

 
    public void InitializeDeck()
    {
        drawPile.Clear();
        discardPile.Clear();

        var build = new List<CardData>(65);
        AddCopies(build, chaosCard, 10);
        AddCopies(build, lightCard, 10);
        AddCopies(build, darkCard, 5);
        AddCopies(build, waterCard, 15);
        AddCopies(build, fireCard, 15);
        AddCopies(build, airCard, 10);

        var shuffled = ShuffleList(build);
        foreach (var c in shuffled) drawPile.Enqueue(c);

        Debug.Log($"[DeckSystem] Mazo inicial creado: {drawPile.Count} cartas. Descarte: {discardPile.Count}");
    }

    private void AddCopies(List<CardData> list, CardData card, int count)
    {
        if (card == null || count <= 0) return;
        for (int i = 0; i < count; i++) list.Add(card);
    }

    /// <summary>
    /// Roba 1 carta. Si el mazo está vacio, intenta mezclar el descarte y continuar.
    /// Si ambos estan vacios, devuelve null.
    /// </summary>
    public CardData DrawCard()
    {
        
        if (drawPile.Count == 0)
        {
            if (discardPile.Count > 0)
            {
                ShuffleDiscardIntoDeck();
            }
            else
            {
                Debug.LogWarning("[DeckSystem] No hay cartas para robar (mazo y descarte vacíos).");
                return null;
            }
        }

        return drawPile.Dequeue();
    }

    /// <summary>
    /// Envia una carta al mazo de descarte (tope del stack).
    /// </summary>
    public void DiscardCard(CardData card)
    {
        if (card == null) return;
        discardPile.Push(card);
    }

    /// <summary>
    /// Mezcla TODO el descarte y lo pasa al mazo 
    /// </summary>
    private void ShuffleDiscardIntoDeck()
    {
        var temp = new List<CardData>(discardPile.Count);
        while (discardPile.Count > 0)
            temp.Add(discardPile.Pop());

        var shuffled = ShuffleList(temp);
        foreach (var c in shuffled) drawPile.Enqueue(c);

        Debug.Log($"[DeckSystem] Mezcla descarte -> mazo. Mazo: {drawPile.Count} | Descarte: {discardPile.Count}");
    }

    private List<CardData> ShuffleList(List<CardData> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }



    /// <summary>
    /// Agrega una carta arriba del mazo (al final de la cola). No mezcla automaticamente. CARTAS A AGREGAR MAS ADELANTE
    /// </summary>
    public void AddCardToDeck(CardData card)
    {
        if (card == null) return;
        drawPile.Enqueue(card);
    }

    /// <summary>
    /// Agrega N copias al mazo No mezcla automáticamente.
    /// </summary>
    public void AddCopiesToDeck(CardData card, int count)
    {
        if (card == null || count <= 0) return;
        for (int i = 0; i < count; i++) drawPile.Enqueue(card);
    }

    // ——— Debug helpers ———
    public int DrawCount => drawPile.Count;
    public int DiscardCount => discardPile.Count;
}
