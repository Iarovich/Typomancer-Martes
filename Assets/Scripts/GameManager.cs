using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [Header("Core refs")]
    public DeckSystem deck;
    public HandManager hand;
    public TypingManager typing;
    public Enemy enemy;
    public Player player;
    [Header("UI (opcional si seguís usando barras en esta escena)")]
    public UIManager uiManager;
    [Header("Escena a cargar al ganar (configurable por nivel)")]
    [SerializeField] private string victorySceneName = "Victory";
    
    // --- Stats básicos para la pantalla de resultados ---
    private float elapsedSeconds;
    private int spellsCast;
    private int correctCasts;
    void Start()
    {
        Time.timeScale = 1f;
        StartCombat();
    }
    void Update()
    {
        elapsedSeconds += Time.deltaTime;
    }
    public void StartCombat()
    {
        if (deck == null) deck = FindObjectOfType<DeckSystem>();
        if (hand == null) hand = FindObjectOfType<HandManager>();
        if (typing == null) typing = FindObjectOfType<TypingManager>();
        if (enemy == null) enemy = FindObjectOfType<Enemy>();
        if (player == null) player = FindObjectOfType<Player>();

        // Reset de stats
        elapsedSeconds = 0f;
        spellsCast = 0;
        correctCasts = 0;


        // Inicializar combate
        deck.InitializeDeck();
        hand.StartHand();


        // Pasar referencias a efectos
        var effectHandler = typing != null ? typing.GetComponent<CardEffectHandler>() : null;
        if (effectHandler != null)
            effectHandler.SetReferences(enemy, player != null ? player.PlayerHealth : FindObjectOfType<Health>());
        
        var eH = enemy != null ? enemy.GetComponent<Health>() : null;
        if (eH != null)
        {
            eH.OnDeath.RemoveListener(OnEnemyDeath);
            eH.OnDeath.AddListener(OnEnemyDeath);
        }
        var pH = player != null ? player.PlayerHealth : null;
        if (pH != null)
        {
            pH.OnDeath.RemoveListener(OnPlayerDeath);
            pH.OnDeath.AddListener(OnPlayerDeath);
        }

    }
    private void HandleAnySpellCast() { spellsCast++; }
    private void HandleCorrectCast() { correctCasts++; }

    // --- Finales de partida ---
    private void OnEnemyDeath()
    {
        PushResult(ResultType.Victory);
        SceneManager.LoadScene(victorySceneName); 
    }
    private void OnPlayerDeath()
    {
        PushResult(ResultType.Defeat);
        SceneManager.LoadScene("Defeat");
    }
   
    private void PushResult(ResultType type)
    {
        var carrier = ResultCarrier.Instance;
        if (carrier == null)
        {
            var go = new GameObject("ResultCarrier");
            carrier = go.AddComponent<ResultCarrier>();
        }
        carrier.result = type;
        
        carrier.lastGameSceneName = SceneManager.GetActiveScene().name;
        
        var eH = enemy != null ? enemy.GetComponent<Health>() : null;
        carrier.enemyMaxHP = eH ? eH.MaxHealth : 0;
        carrier.enemyFinalHP = eH ? eH.CurrentHealth : 0;
        var pH = player != null ? player.PlayerHealth : null;
        carrier.playerMaxHP = pH ? pH.MaxHealth : 0;
        carrier.playerFinalHP = pH ? pH.CurrentHealth : 0;
       

        carrier.elapsedSeconds = elapsedSeconds;
    }
}