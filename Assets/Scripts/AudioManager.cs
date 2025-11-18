using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //Singleton
    public static AudioManager Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInit()
    {
        if (Instance == null)
        {
            var go = new GameObject("AudioManager");
            go.AddComponent<AudioManager>();
        }
    }

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;      
    [SerializeField] private AudioMixerGroup sfxMixer;   

    [Header("Comportamiento")]
    [SerializeField] private bool force2D = true;
    [SerializeField] private bool snapToListenerOnSceneLoad = true;
    [SerializeField] private bool dontDestroyOnLoad = true;

    void Awake()
    {
        //Singleton - garantiza una unica instancia global
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

        EnsureSFXSource();

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;

    public void PlayBGM(AudioClip clip, float volume = 1f, bool loop = true)
    {
        if (clip == null) return;

        
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.playOnAwake = false;
        }

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.volume = Mathf.Clamp01(volume);
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        
        if (!snapToListenerOnSceneLoad) return;
        if (sfxSource != null && sfxSource.spatialBlend > 0f)
        {
            var listener = FindFirstObjectByType<AudioListener>();
            sfxSource.transform.position =
                listener != null ? listener.transform.position :
                (Camera.main != null ? Camera.main.transform.position : Vector3.zero);
        }
    }

    private void EnsureSFXSource()
    {
       
        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
        }

        
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.ignoreListenerPause = true;
        sfxSource.outputAudioMixerGroup = sfxMixer;
        sfxSource.spatialBlend = force2D ? 0f : sfxSource.spatialBlend; 
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

       
        if (sfxSource == null) EnsureSFXSource();
        if (sfxSource == null) return; 

        
        if (sfxSource.spatialBlend > 0f)
        {
            var listener = FindFirstObjectByType<AudioListener>();
            sfxSource.transform.position =
                listener != null ? listener.transform.position :
                (Camera.main != null ? Camera.main.transform.position : Vector3.zero);
        }

        
        try
        {
            sfxSource.Stop();
            sfxSource.clip = clip;
            sfxSource.volume = Mathf.Clamp01(volume);
            sfxSource.Play();
        }
        catch {  }




    }
}
