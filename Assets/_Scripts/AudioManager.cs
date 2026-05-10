using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip pickupTrashSound;
    [SerializeField] private AudioClip discardTrashSound;

    private bool isMuted = false;
    public bool IsMuted => isMuted;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null) return;
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }

    public void PlayGameOver()
    {
        sfxSource.PlayOneShot(gameOverSound);
    }

    public void PlayPickupTrash()
    {
        sfxSource.PlayOneShot(pickupTrashSound);
    }

    public void PlayDiscardTrash()
    {
        sfxSource.PlayOneShot(discardTrashSound);
    }
}