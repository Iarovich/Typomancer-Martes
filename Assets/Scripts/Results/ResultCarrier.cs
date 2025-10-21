using UnityEngine;

public enum ResultType { Victory, Defeat }

public class ResultCarrier : MonoBehaviour
{
    public static ResultCarrier Instance { get; private set; }

    [Header("Payload")]
    public ResultType result = ResultType.Victory;
    public int enemyMaxHP;
    public int enemyFinalHP;
    public int playerMaxHP;
    public int playerFinalHP;
    public string lastGameSceneName = "Game2";

       
    public float elapsedSeconds;  // duración de la run

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetData()
    {
        result = ResultType.Victory;
        enemyMaxHP = enemyFinalHP = 0;
        playerMaxHP = playerFinalHP = 0;
       
        elapsedSeconds = 0f;
    }

  
}
