using System.Collections.Generic;
using UnityEngine;
public class HandManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private DeckSystem deckSystem;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject cardPrefab;
    [Header("Tamaños de mano")]
    [SerializeField] private int initialHandSize = 6; 
    [SerializeField] private int maxHandSize = 8;     
    private readonly List<CardView> handViews = new List<CardView>();
    void Start()
    {
        if (deckSystem == null) deckSystem = FindFirstObjectByType<DeckSystem>();
    }
    // Inicio de combate: roba SOLO initialHandSize 
    public void StartHand()
    {
        if (deckSystem == null) { Debug.LogError("[HandManager] deckSystem es NULL."); return; }
        if (cardPrefab == null) { Debug.LogError("[HandManager] cardPrefab es NULL."); return; }
        if (cardContainer == null) { Debug.LogError("[HandManager] cardContainer es NULL."); return; }
        ClearHand();
        DrawUpTo(initialHandSize);
        Debug.Log($"[HandManager] Mano inicial: {handViews.Count} cartas (max {maxHandSize}).");
    }
   
    private int SpaceLeft() => Mathf.Max(0, maxHandSize - handViews.Count);
  
    public void DrawToHand()
    {
        if (SpaceLeft() <= 0) return;
        var card = deckSystem.DrawCard(); // mezcla descarte->mazo si está vacío
        if (card == null)
        {
            Debug.LogWarning("[HandManager] DrawCard() devolvió null (mazo+descarte vacíos).");
            return;
        }
        var go = Object.Instantiate(cardPrefab, cardContainer);
        var cv = go.GetComponent<CardView>();
        cv.SetData(card);
        handViews.Add(cv);
    }
   
    public void DrawN(int n)
    {
        int toDraw = Mathf.Min(n, SpaceLeft());
        for (int i = 0; i < toDraw; i++) DrawToHand();
    }
   
    private void DrawUpTo(int targetCount)
    {
        int need = Mathf.Clamp(targetCount - handViews.Count, 0, SpaceLeft());
        for (int i = 0; i < need; i++) DrawToHand();
    }
   
    public void UseCard(CardView cv)
    {
        if (cv == null) return;
        var data = cv.GetData();
        if (data == null) return;
       
        deckSystem.DiscardCard(data);
        
        cv.MarkUsed();
        handViews.Remove(cv);
        Object.Destroy(cv.gameObject);
        // Si quedo vacia roba hasta 3 
        if (handViews.Count == 0)
            DrawN(3);
        else
            DrawN(1); // reponer 1 normalmente
    }
    // Descartar aleatoria (DARK) 
    public void DiscardRandomFromHand()
    {
        if (handViews.Count == 0) return;
        int idx = Random.Range(0, handViews.Count);
        var cv = handViews[idx];
        if (cv == null) return;
        var data = cv.GetData();
        deckSystem.DiscardCard(data);
        cv.MarkUsed();
        handViews.RemoveAt(idx);
        Object.Destroy(cv.gameObject);
      
        if (handViews.Count == 0)
            DrawN(3);
    }

    public CardView FindCardMatchingName(string typed)
    {
        if (string.IsNullOrEmpty(typed)) return null;
        foreach (var v in handViews)
        {
            if (v == null) continue;
            var d = v.GetData();
            if (d == null) continue;
            if (string.Equals(d.cardName, typed, System.StringComparison.InvariantCultureIgnoreCase))
                return v;
        }
        return null;
    }
   
    public void ClearHand()
    {
        foreach (var v in handViews) if (v != null) Object.Destroy(v.gameObject);
        handViews.Clear();
        if (cardContainer != null)
        {
            for (int i = cardContainer.childCount - 1; i >= 0; i--)
                Object.Destroy(cardContainer.GetChild(i).gameObject);
        }
    }
    public IReadOnlyList<CardView> GetHand() => handViews;
   
    public int CurrentCount => handViews.Count;
    public int MaxHandSize => maxHandSize;
    public int InitialHandSize => initialHandSize;
}