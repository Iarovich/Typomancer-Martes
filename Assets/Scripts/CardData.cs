using UnityEngine;

[CreateAssetMenu(fileName = "CardData_", menuName = "Typomancer/CardData")]
public class CardData : ScriptableObject
{
    public string cardName; 
    public CardElement element;
    public Sprite icon;
    [TextArea] public string description;
    public int power = 10; 
    public GameObject vfxPrefab;

    public AudioClip sfxClip;
    [Range(0f, 1f)] public float sfxVolume = 0.35f;
}
