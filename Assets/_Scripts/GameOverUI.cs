using System;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private void Start() {
        GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;

        Hide();
    }

    private void GameManagerOnStateChanged(object sender, EventArgs e)
    {
        if(GameManager.Instance.IsGameOver()) {
            Show();
        } else {
            Hide();
        }
    }

    
    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    } 
}
