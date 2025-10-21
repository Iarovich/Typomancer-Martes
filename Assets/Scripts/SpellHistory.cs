using System.Collections.Generic;
using UnityEngine;

public class SpellHistory : MonoBehaviour
{
    private Stack<CardData> history = new Stack<CardData>();

    public void AddToHistory(CardData card)
    {
        if (card == null) return;
        history.Push(card);
    }

    public CardData UndoLast()
    {
        if (history.Count == 0) return null;
        return history.Pop();
    }

    public CardData PeekLast()
    {
        if (history.Count == 0) return null;
        return history.Peek();
    }

    public int Count => history.Count;
}
