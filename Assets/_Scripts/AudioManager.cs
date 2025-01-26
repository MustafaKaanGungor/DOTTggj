using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private const string PLAYER_PREFS_VOLUME = "MusicVolume";

    private AudioSource audioSource;
    private float volume = 0.2f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();

            volume = PlayerPrefs.GetFloat(PLAYER_PREFS_VOLUME, volume);
            audioSource.volume = volume;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeVolume(float sliderVolume)
    {
        volume = sliderVolume;
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
