using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] private GameObject gameOverPanel;
    private enum State {
        WaitingToStart,
        GamePlaying,
        GameOver
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
                if(Boss.Instance.IsBossDead() || Player.Instance.IsPlayerDead()) {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
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
