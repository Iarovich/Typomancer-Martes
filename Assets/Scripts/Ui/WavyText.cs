using UnityEngine;
using TMPro;
[RequireComponent(typeof(TMP_Text))]
public class WavyTextTMP_Safe : MonoBehaviour
{
    [Header("Onda")]
    [Tooltip("Altura de la onda en píxeles")]
    public float amplitude = 10f;
    [Tooltip("Frecuencia de la onda")]
    public float frequency = 1f;
    [Tooltip("Velocidad del desplazamiento")]
    public float speed = 2f;
    [Tooltip("Desfase entre letras (radianes)")]
    public float phasePerChar = 0.4f;
    [Header("Opcional")]
    public bool horizontalRipple = false;
    public float horizontalScale = 0.25f;
    [Header("Preview en Editor")]
    [Tooltip("Permite ver la animación en modo Editor. Activalo solo si lo necesitás.")]
    public bool previewInEditMode = false;
    TMP_Text tmp;
    TMP_TextInfo textInfo;
    TMP_MeshInfo[] baseMeshInfo;     // baseline (cache) de vértices
    bool cacheReady;
    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }
    void OnEnable()
    {
        BuildCache();
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }
    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }
    void OnValidate()
    {
        frequency = Mathf.Max(0.1f, frequency);
    }
    void OnTextChanged(Object obj)
    {
        if (obj == tmp) BuildCache();
    }
    void BuildCache()
    {
        if (tmp == null) return;
        tmp.ForceMeshUpdate();
        textInfo = tmp.textInfo;
        // copiamos una sola vez el baseline de cada submesh
        baseMeshInfo = textInfo.CopyMeshInfoVertexData();
        cacheReady = true;
    }
    void LateUpdate()
    {
        if (!cacheReady || tmp == null) return;
        // Solo animar si estamos jugando, o si el usuario habilitó preview
        if (!Application.isPlaying && !previewInEditMode) return;
        // si el layout cambió (cambio de fuente, tamaño, word wrap, etc.), reconstruir cache
        if (textInfo == null || textInfo.characterCount != tmp.textInfo.characterCount ||
            baseMeshInfo == null || baseMeshInfo.Length != tmp.textInfo.meshInfo.Length)
        {
            BuildCache();
            return;
        }
        // restaurar baseline antes de aplicar offsets
        for (int m = 0; m < tmp.textInfo.meshInfo.Length; m++)
        {
            var src = baseMeshInfo[m].vertices;
            var dst = tmp.textInfo.meshInfo[m].vertices;
            if (src == null || dst == null || src.Length != dst.Length) continue;
            System.Array.Copy(src, dst, src.Length);
        }
        float t = Application.isPlaying ? Time.unscaledTime : Time.realtimeSinceStartup;
        int charCount = tmp.textInfo.characterCount;
        for (int i = 0; i < charCount; i++)
        {
            var ch = tmp.textInfo.characterInfo[i];
            if (!ch.isVisible) continue;
            int mi = ch.materialReferenceIndex;
            int vi = ch.vertexIndex;
            var verts = tmp.textInfo.meshInfo[mi].vertices;
            float phase = t * speed + i * phasePerChar;
            float y = Mathf.Sin(phase * frequency) * amplitude;
            float x = horizontalRipple ? Mathf.Cos(phase * frequency) * amplitude * 0.1f * horizontalScale : 0f;
            Vector3 off = new Vector3(x, y, 0f);
            verts[vi + 0] += off;
            verts[vi + 1] += off;
            verts[vi + 2] += off;
            verts[vi + 3] += off;
        }
        // subir geometría modificada
        for (int m = 0; m < tmp.textInfo.meshInfo.Length; m++)
        {
            var mi = tmp.textInfo.meshInfo[m];
            mi.mesh.vertices = mi.vertices;
            tmp.UpdateGeometry(mi.mesh, m);
        }
    }
}