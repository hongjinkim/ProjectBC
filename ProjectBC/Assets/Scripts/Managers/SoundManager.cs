using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip hitSound;

    private bool isBattleSoundsEnabled = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayHitSound()
    {
        if (isBattleSoundsEnabled)
        {
            effectsSource.PlayOneShot(hitSound);
        }
    }

    public void EnableBattleSounds()
    {
        isBattleSoundsEnabled = true;
    }

    public void DisableBattleSounds()
    {
        isBattleSoundsEnabled = false;
    }
}