using UnityEngine;
using UnityEngine.SceneManagement;

// Facade - Proporciona una interfaz simplificada para acceder a multiples sistemas complejos
public class GameManager : MonoBehaviour
{
    [Header("Core refs")]
    public DeckSystem deck;
    public HandManager hand;
    public TypingManager typing;
    public Enemy enemy;
    public Player player;
    public PlayerMemento memento;
    [Header("UI (opcional si segu�s usando barras en esta escena)")]
    public UIManager uiManager;
    [Header("Escena a cargar al ganar (configurable por nivel)")]
    [SerializeField] private string victorySceneName = "Victory";
    
    //Stats b�sicos para la pantalla de resultados
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
    //Facade - Método que simplifica la inicialización de múltiples sistemas
    public void StartCombat()
    {
        //Facade - Obtiene referencias a sistemas complejos (oculta la complejidad)
        if (deck == null) deck = FindFirstObjectByType<DeckSystem>();
        if (hand == null) hand = FindFirstObjectByType<HandManager>();
        if (typing == null) typing = FindFirstObjectByType<TypingManager>();
        if (enemy == null) enemy = FindFirstObjectByType<Enemy>();
        if (player == null) player = FindFirstObjectByType<Player>();

        // Reset de stats
        elapsedSeconds = 0f;
        spellsCast = 0;
        correctCasts = 0;

        //Facade - Coordina la inicializacion de sistemas (simplifica el uso)
        //Inicializar combate
        deck.InitializeDeck();
        hand.StartHand();

        //Pasar referencias a efectos
        var effectHandler = typing != null ? typing.GetComponent<CardEffectHandler>() : null;
        if (effectHandler != null)
            effectHandler.SetReferences(enemy, player != null ? player.PlayerHealth : FindFirstObjectByType<Health>());
        
        //Eventos de muerte
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

    //Finales de partida
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