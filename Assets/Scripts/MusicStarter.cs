using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;
    [SerializeField] private float volume = 0.7f;

    void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBGM(bgm, volume);
    }
}
