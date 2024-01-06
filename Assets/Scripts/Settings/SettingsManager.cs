using UnityEngine;

/// <summary>
/// The Settings Manager manages settings that have to be persisted over multiple
/// game Sessions and versions.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    /*
     * We use a simple version hash here that we will take into account for
     * our stored keys in PlayerPrefs. Makes things easier later if we would have
     * to migrate stuff
     * 
     * TODO: Maybe something for tooling. Use a proper build hash in future?
     */
    private string versionHash = "1.00";

    private float musicVolume;
    private float sfxVolume;
    private float bulletCamIntensity;

    public float GetMusicVolume() => musicVolume;

    public float GetSfxVolume() => sfxVolume;

    public float GetBulletCamIntensity() => bulletCamIntensity;

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp(volume, 0f, 1f);
        Set("MusicVolume", musicVolume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp(volume, 0f, 1f);
        Set("SfxVolume", sfxVolume);
    }

    public void SetBulletCamIntensity(float intensity)
    {
        bulletCamIntensity = Mathf.Clamp(intensity, 0f, 1f);
        Set("BulletCamIntensity", bulletCamIntensity);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        LoadStoredSettings();
    }

    /// <summary>
    /// Loads our settings stored in PlayerPrefs. A proper default value
    /// will be used if the key would not exist yet
    /// </summary>
    private void LoadStoredSettings()
    {
        musicVolume = Get("MusicVolume", 0.25f);
        sfxVolume = Get("SfxVolume", 0.25f);
        bulletCamIntensity = Get("BulletCamIntensity", 0.5f);
    }

    private void Set(string key, float value)
    {
        PlayerPrefs.SetFloat($"{versionHash}/{key}", value);
    }

    private float Get(string key, float defaultValue)
    {
        if (!PlayerPrefs.HasKey($"{versionHash}/{key}"))
        {
            Set(key, defaultValue);
        }
        return PlayerPrefs.GetFloat($"{versionHash}/{key}", defaultValue);
    }
}
