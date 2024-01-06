using UnityEngine;

public class AudioManager : MonoBehaviour, IBulletCamListener
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private AudioClip soundTrack;


    private float musicVolume;
    private float sfxVolume;

    private float tempMusicVolume;

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        // Would update the volume of current playing music
        GetComponentInChildren<AudioSource>().volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
    }

    /// <summary>
    /// Plays an audioClip on specified gameObject.
    /// </summary>
    /// <param name="audioClip">audio clip</param>
    /// <param name="gameObject">Game Object</param>
    /// <param name="loop">Loop the audio clip?</param>
    /// <param name="volume">volume of audio clip</param>
    public void PlaySound(AudioClip audioClip, GameObject gameObject, bool loop = false, float volume = 1f)
    {
        AudioSource audioSource;

        if (!gameObject.TryGetComponent(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (loop)
        {
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(audioClip);
        }
        audioSource.volume = volume;
    }

    /// <summary>
    /// Plays an audioClip on AudioManager. A gameObject will be created
    /// on AudioManager.
    /// </summary>
    /// <param name="audioClip">audio clip</param>
    /// <param name="loop">Loop the audio clip?</param>
    /// <param name="volume">volume of audio clip</param>
    /// <param name="destroyOnEnd">Should the gameObject be destroyed after the sound finieshed playing?</param>
    public void PlaySound(AudioClip audioClip, bool loop = false, float volume = 1f, bool destroyOnEnd = false)
    {
        GameObject gameObject = new GameObject();

        gameObject.transform.SetParent(transform);

        if (destroyOnEnd)
        {
            Destroy(gameObject, audioClip.length);
        }

        PlaySound(audioClip, gameObject, loop, volume);
    }

    /// <summary>
    /// Plays Music on AudioManager. Will be looped.
    /// The Music will be played trough a child GameObject on AudioManager called Music.
    /// Music will be swapped out if theres alredy music.
    /// </summary>
    /// <param name="audioClip">Audio Clip</param>
    public void PlayMusic(AudioClip audioClip)
    {
        Transform child = gameObject.transform.Find("Music");

        if (child == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = "Music";
            gameObject.transform.SetParent(transform);

            PlaySound(audioClip, gameObject, true, musicVolume);
        }
        else
        {
            PlaySound(audioClip, child.gameObject, true, musicVolume);
        }
    }

    /// <summary>
    /// Plays a Sound Effect on specified Game Object. Will only be played once.
    /// </summary>
    /// <param name="gameObject">Game Object</param>
    /// <param name="audioClip">Audio Clip</param>
    public void PlaySfx(AudioClip audioClip, GameObject gameObject)
    {
        PlaySound(audioClip, gameObject, volume: sfxVolume);
    }

    /// <summary>
    /// Plays a Sound Effect on AudioManager. Will only be played once.
    /// The SFX will be played trough a child gameObject on AudioManager that
    /// will be destroyed after the SFX ends.
    /// </summary>
    /// <param name="audioClip">Audio Clip</param>
    public void PlaySfx(AudioClip audioClip)
    {
        PlaySound(audioClip, volume: sfxVolume, destroyOnEnd: true);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        // This will enforce that the AudioManager still exists if we switch or
        // reload a scene.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicVolume = SettingsManager.Instance.GetMusicVolume();
        sfxVolume = SettingsManager.Instance.GetSfxVolume();

        PlayMusic(soundTrack);
    }

    void IBulletCamListener.OnBulletCamStart(Bullet targetBullet, RaycastHit targetHit)
    {
        tempMusicVolume = musicVolume;
        SetMusicVolume(0.01f);
    }

    void IBulletCamListener.OnBulletCamEnd(Bullet targetBullet, RaycastHit targetHit)
    {
        SetMusicVolume(tempMusicVolume);
    }
}
