using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CardView : MonoBehaviour
{
    public TMP_Text nameText;
    public Image iconImage;
    public Image border;
    public GameObject usedOverlay;

    private CardData data;

    public void SetData(CardData cd)
    {
        data = cd;
        if (nameText != null) nameText.text = cd.cardName;
        if (iconImage != null) iconImage.sprite = cd.icon;
        usedOverlay?.SetActive(false);
    }

    public CardData GetData() => data;

    public void MarkUsed()
    {
        usedOverlay?.SetActive(true);
        
    }
}
