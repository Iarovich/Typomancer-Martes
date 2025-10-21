
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class TypingManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private HandManager handManager;
    [SerializeField] private CardEffectHandler effectHandler;

    void OnEnable()
    {
        StartCoroutine(FocusNextFrame());
    }

    IEnumerator FocusNextFrame()
    {
        yield return null; 
        if (inputField != null)
        {
            EventSystem.current?.SetSelectedGameObject(inputField.gameObject);
            inputField.ActivateInputField();
            inputField.caretPosition = inputField.text.Length;
        }
    }

    void Start()
    {
        if (inputField == null) Debug.LogError("[TypingManager] Falta TMP_InputField.");
        if (handManager == null) Debug.LogError("[TypingManager] Falta HandManager.");
        if (effectHandler == null) Debug.LogWarning("[TypingManager] Falta CardEffectHandler.");

        if (inputField != null)
        {
            inputField.onSubmit.RemoveListener(OnSubmit);
            inputField.onSubmit.AddListener(OnSubmit);
            StartCoroutine(FocusNextFrame());
        }
    }

    private void OnSubmit(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
            TryActivate(text.Trim());

        // limpiar y volver a enfocar SIEMPRE
        inputField.text = "";
        StartCoroutine(FocusNextFrame());
    }

    private void TryActivate(string typed)
    {
        // 0) Validaciones base
        if (handManager == null) { Debug.LogError("[TypingManager] handManager es NULL."); return; }

        // 1) Buscar coincidencia exacta en la mano
        var match = handManager.FindCardMatchingName(typed);
        if (match == null)
        {
            var hand = handManager.GetHand();
            string names = hand == null ? "null" : string.Join(", ", System.Array.ConvertAll(hand.ToArray(), cv => cv?.GetData()?.cardName ?? "(null)"));
            Debug.LogWarning($"[TypingManager] No hay carta que coincida con '{typed}'. Mano: [{names}]");
            return;
        }

        // 2) Obtener datos de la carta
        var data = match.GetData();  
        if (data == null)
        {
            Debug.LogError("[TypingManager] match.GetData() devolvió NULL. ¿Se llamó CardView.SetData()?");
            return;
        }

        // 3) Handler de efectos
        if (effectHandler == null)
        {
            Debug.LogError("[TypingManager] effectHandler es NULL. Asignalo en el Inspector o vía GameManager.SetReferences().");
            return;
        }

        // 4) Aplicar efecto y usar carta
        effectHandler.ApplyEffect(data);
        handManager.UseCard(match);

        // 5) Regla especial para DARK: descartar 1 carta extra al azar
        if (data.element == CardElement.Dark)
        {
            handManager.DiscardRandomFromHand();
        }
    }

}

