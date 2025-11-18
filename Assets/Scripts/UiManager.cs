using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Patron:Singleton
    public static UIManager Instance { get; private set; }

    // Referencias de UI
    [Header("Referencias UI")]
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text enemyHealthText;
    [SerializeField] private TMP_Text spellInputText;
    [SerializeField] private TMP_Text deckCountText;
    [SerializeField] private TMP_Text discardCountText;
    [SerializeField] private TMP_Text messageText;

    private void Awake()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void UpdatePlayerHealth(float current, float max)
    {
        if (playerHealthBar)
        {
            playerHealthBar.value = current / max;
        }
        if (playerHealthText)
        {
            playerHealthText.text = $"{current}/{max}";
        }
    }

    public void UpdateEnemyHealth(float current, float max)
    {
        if (enemyHealthBar)
        {
            enemyHealthBar.value = current / max;
        }
        if (enemyHealthText)
        {
            enemyHealthText.text = $"{current}/{max}";
        }
    }

    public void UpdateDeckCount(int count)
    {
        if (deckCountText)
        {
            deckCountText.text = $"Deck: {count}";
        }
    }

    public void UpdateDiscardCount(int count)
    {
        if (discardCountText)
        {
            discardCountText.text = $"Discard: {count}";
        }
    }

    public void UpdateSpellInput(string text)
    {
        if (spellInputText)
        {
            spellInputText.text = text;
        }
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        if (messageText)
        {
            messageText.text = message;
            CancelInvoke(nameof(ClearMessage));
            Invoke(nameof(ClearMessage), duration);
        }
    }

    private void ClearMessage()
    {
        if (messageText)
        {
            messageText.text = "";
        }
    }
}
