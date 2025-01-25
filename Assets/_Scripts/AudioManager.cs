using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class AudioManager:MonoBehaviour
{
    public static AudioManager instance;
    private Scene currentScene;
    private Scene MainMenu;
    [SerializeField] private float slowedTime;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        MainMenu = SceneManager.GetSceneByBuildIndex(0);
        if(currentScene == MainMenu)
        {
            Time.timeScale = slowedTime;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
