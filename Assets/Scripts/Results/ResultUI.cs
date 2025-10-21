using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [Header("UI refs")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text statsText;

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Start()
    {
        Time.timeScale = 1f; 

        var c = ResultCarrier.Instance;
        if (c == null)
        {
            
            if (titleText) titleText.text = "Resultado";
            if (statsText) statsText.text = "No hay datos.";
            return;
        }

        if (titleText)
            titleText.text = (c.result == ResultType.Victory) ? "¡Victoria!" : "Derrota…";

        if (statsText)
        {
            string timeStr = $"{Mathf.FloorToInt(c.elapsedSeconds / 60f):00}:{Mathf.FloorToInt(c.elapsedSeconds % 60f):00}";

            statsText.text =
                $"Tiempo: {timeStr}\n" +
                $"HP Jugador: {c.playerFinalHP} / {c.playerMaxHP}\n" +
                $"HP Enemigo: {c.enemyFinalHP} / {c.enemyMaxHP}";
        }
    }

    // Botones
    public void OnRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // (Opcional) siguiente
    public void OnNext()
    {
        // aquí podrías poner lógica para elegir el próximo nivel/oleada
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}
