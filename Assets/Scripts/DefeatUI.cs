using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class DefeatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    void Start()
    {
        Time.timeScale = 1f;
        if (titleText) titleText.text = "Derrota…";
        var c = ResultCarrier.Instance;
        if (statsText)
        {
            if (c == null)
            {
                statsText.text = "No hay datos.";
            }
            else
            {

                string timeStr = $"{Mathf.FloorToInt(c.elapsedSeconds / 60f):00}:{Mathf.FloorToInt(c.elapsedSeconds % 60f):00}";
                statsText.text =
                    $"Tiempo: {timeStr}\n" +
                    $"HP Jugador: {c.playerFinalHP} / {c.playerMaxHP}\n" +
                    $"HP Enemigo: {c.enemyFinalHP} / {c.enemyMaxHP}";
            }
        }
    }
    public void OnRetry() { SceneManager.LoadScene(gameSceneName); }
    public void OnMainMenu() { SceneManager.LoadScene(mainMenuSceneName); }
}