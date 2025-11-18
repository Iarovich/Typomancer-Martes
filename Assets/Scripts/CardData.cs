using UnityEngine;

//Type Object - define tipos de cartas con datos compartidos
//Flyweight - multiples instancias comparten el mismo CardData (ScriptableObject)
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
