using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI GameOverText;
    private enum State {
        WaitingToStart,
        GamePlaying,
        GameOver,
        PlayerWin
    }
    private State state;
    private float gameStartTimer;
    private float gameStartTimerMax;

    public event EventHandler OnStateChanged;

    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start() {
        //oyunun durması eventleri koy

    }

    private void Update() {
        switch(state) {
            case State.WaitingToStart:
                gameStartTimer += Time.deltaTime;
                if(gameStartTimer >= gameStartTimerMax) {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                if(Boss.Instance.IsBossDead()) {
                    state = State.PlayerWin;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                if (Player.Instance.IsPlayerDead())
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                GameOverText.text = "Game Over";
                gameOverPanel.SetActive(true);
                break;
            case State.PlayerWin:
                GameOverText.text = "You Win";
                gameOverPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    public bool IsWaitingForStart() {
        return state == State.WaitingToStart;
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
