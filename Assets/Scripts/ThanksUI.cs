using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class ThanksUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    void Start()
    {
        Time.timeScale = 1f;
        if (titleText) titleText.text = "Thanks for playing!";
        var c = ResultCarrier.Instance;
        if (statsText)
        {
            if (c == null)
            {
                statsText.text = "No data.";
            }
            else
            {
                
                string timeStr = $"{Mathf.FloorToInt(c.elapsedSeconds / 60f):00}:{Mathf.FloorToInt(c.elapsedSeconds % 60f):00}";
                statsText.text =
                    $"Time: {timeStr}\n" +
                    $"Player HP: {c.playerFinalHP} / {c.playerMaxHP}\n" +
                    $"Enemy HP: {c.enemyFinalHP} / {c.enemyMaxHP}";
            }
        }
    }
    // Reintenta el último nivel jugado 
    public void OnRetry()
    {
        var c = ResultCarrier.Instance;
        string sceneToLoad = (c != null && !string.IsNullOrEmpty(c.lastGameSceneName)) ? c.lastGameSceneName : "Game";
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }
    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}