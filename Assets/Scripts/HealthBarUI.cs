using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Health health;

    [Header("Images")]
    [SerializeField] private Image redBG;
    [SerializeField] private Image greenFill;

    [Header("Chip effect (opcional)")]
    [SerializeField] private bool useChip = true;
    [SerializeField] private float chipDelay = 0.15f;
    [SerializeField] private float chipSpeed = 1.5f;

    private float targetFill01 = 1f;
    private float chipTimer = 0f;

    void OnEnable()
    {
        Subscribe();
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    public void SetHealth(Health h)
    {
        Unsubscribe();
        health = h;
        Subscribe();
        InitializeBar();
    }

    private void Subscribe()
    {
        if (health != null)
        {
            health.OnHPChanged.AddListener(OnHPChanged);
            InitializeBar();
        }
    }

    private void Unsubscribe()
    {
        if (health != null)
            health.OnHPChanged.RemoveListener(OnHPChanged);
    }

    private void InitializeBar()
    {
        float f = (health != null && health.MaxHealth > 0)
            ? (float)health.CurrentHealth / health.MaxHealth
            : 1f;

        targetFill01 = Mathf.Clamp01(f);

        if (greenFill != null)
            greenFill.fillAmount = targetFill01;
    }

    private void OnHPChanged(int current)
    {
        if (health == null || health.MaxHealth <= 0) return;

        targetFill01 = Mathf.Clamp01((float)current / health.MaxHealth);

        if (greenFill != null)
            greenFill.fillAmount = targetFill01;
    }

    void Update()
    {
        if (chipTimer > 0f)
        {
            chipTimer -= Time.deltaTime;
            return;
        }
    }
}
