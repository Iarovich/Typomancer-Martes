using UnityEngine;
#if UNITY_VISUAL_EFFECT_GRAPH
using UnityEngine.VFX;
#endif

public class CardEffectHandler : MonoBehaviour
{
    [Header("Referencias de combate")]
    [SerializeField] private Enemy currentEnemy;
    [SerializeField] private Health playerHealth;
    [SerializeField] private SpellHistory spellHistory;
    [SerializeField] private HandManager handManager;

    [Header("Anclajes (World)")]
    [SerializeField] private Transform enemyAnchor;
    [SerializeField] private Transform playerAnchor;
    [SerializeField] private Transform worldFXParent;

    [Header("UI FX (opcional, si algunos VFX son UI)")]
    [SerializeField] private RectTransform uiFXParent;

    [Header("Defaults VFX")]
    [SerializeField] private float defaultVfxLifetime = 3f;
    [SerializeField] private string vfxSortingLayer = "Default";
    [SerializeField] private int vfxSortingOrder = 0;

    public void SetReferences(Enemy enemyRef, Health playerHealthRef)
    {
        currentEnemy = enemyRef;
        playerHealth = playerHealthRef;

        if (enemyAnchor == null && enemyRef != null) enemyAnchor = enemyRef.transform;
        if (playerAnchor == null && playerHealthRef != null) playerAnchor = playerHealthRef.transform;

        if (handManager == null)
            handManager = FindFirstObjectByType<HandManager>();
    }

    public void ApplyEffect(CardData card)
    {
        if (card == null) return;
        spellHistory?.AddToHistory(card);

        //Factory - crea el comando apropiado segun el elemento
        ICardCommand command = CardCommandFactory.Create(card.element);
        if (command == null)
        {
            Debug.LogWarning($"[CardEffectHandler] No hay comando definido para {card.element}");
            return;
        }

        // Command - ejecuta el comando encapsulado
        command.Execute(currentEnemy, playerHealth, handManager, card, this);

        AudioManager.Instance?.PlaySFX(card.sfxClip, card.sfxVolume);
    }

    public void SpawnEffect(CardData card)
    {
        if (card == null || card.vfxPrefab == null) return;

        Transform anchor = GetDefaultAnchorFor(card.element);
        SpawnVFX(card.vfxPrefab, anchor, Vector3.zero, defaultVfxLifetime);
    }

    private Transform GetDefaultAnchorFor(CardElement element)
    {
        return element == CardElement.Light
            ? (playerAnchor != null ? playerAnchor : transform)
            : (enemyAnchor != null ? enemyAnchor : transform);
    }

    private void SpawnVFX(GameObject prefab, Transform anchor, Vector3 offset, float destroyAfter)
    {
        if (prefab == null) return;

        bool isUI = prefab.GetComponent<RectTransform>() != null;

        if (isUI)
        {
            if (uiFXParent == null)
            {
                Debug.LogWarning("[VFX] Prefab UI pero uiFXParent es NULL. No se instancia.");
                return;
            }

            var uiGo = Instantiate(prefab, uiFXParent);
            var rt = uiGo.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.localScale = Vector3.one;

            if (anchor != null && Camera.main != null)
            {
                Vector3 screen = Camera.main.WorldToScreenPoint(anchor.position + offset);
                rt.position = screen;
            }
            else
            {
                rt.anchoredPosition = Vector2.zero;
            }

            if (destroyAfter > 0f) Destroy(uiGo, destroyAfter);
            return;
        }

        Vector3 pos = (anchor != null ? anchor.position : Vector3.zero) + offset;
        Quaternion rot = (anchor != null ? anchor.rotation : Quaternion.identity);
        Transform parent = worldFXParent != null ? worldFXParent : (anchor != null ? anchor : null);

        var go = Instantiate(prefab, pos, rot, parent);
        go.transform.localScale = Vector3.one;

        foreach (var r in go.GetComponentsInChildren<Renderer>(true))
        {
            r.sortingLayerName = vfxSortingLayer;
            r.sortingOrder = vfxSortingOrder;
        }

        var ps = go.GetComponentInChildren<ParticleSystem>(true);
        if (ps != null) { ps.Clear(true); ps.Play(true); }

#if UNITY_VISUAL_EFFECT_GRAPH
        var vfx = go.GetComponentInChildren<VisualEffect>(true);
        if (vfx != null) vfx.Play();
#endif

        if (destroyAfter > 0f) Destroy(go, destroyAfter);
    }
}
