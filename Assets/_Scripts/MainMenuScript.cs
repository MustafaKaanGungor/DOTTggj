using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
//Ana menü Allaha emanet çalışıyor ellemeyin sonra bir ayar daha çekecem
//Brightness Çalışmıyor
//abdül ben scene manager1 adlı bir script oluşturdum . orda oyun scene nine geçme ve exit fonksiyonları oluşturdum onları kullan , hepsi tek bir yerde olsun

public class MainMenuScript : MonoBehaviour
{
    public string gameLoad;
    [Header("Brightness Settings")]
    [SerializeField] private Volume brightnessVolume;
    private ColorAdjustments brightnessvalue;
    [SerializeField] private Slider brightnessSlider = null;
    [Header("Volume Settings")]
    [SerializeField] private TextMeshProUGUI volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Sprite[] images;
    private int spriteIndex = 0;
    [SerializeField] float timeToWait;
    [SerializeField] private VolumeProfile profile;

    void Start()
    {
        profile.TryGet(out brightnessvalue);

        AudioListener.volume = AudioManager.Instance.GetVolume();
        volumeSlider.onValueChanged.AddListener((volume) =>
        {
            AudioManager.Instance.ChangeVolume(volume);
            volumeTextValue.text = Mathf.RoundToInt(volume).ToString();
        });
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(gameLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void Brightness(float brightness)
    {
        profile.TryGet(out brightnessvalue);
        brightnessvalue.postExposure.value = brightness;
        PlayerPrefs.SetFloat("Brightness",brightness);
    }

    public void DontDestroyAudioManager(GameObject AudioManager)
    {
        DontDestroyOnLoad(AudioManager);
    }
}
