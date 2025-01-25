using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;} 

    private enum State {
        WaitingToStart,
        GamePlaying,
        GameOver
    }

    private State state;

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
                break;
            case State.GamePlaying:
                
                break;
            case State.GameOver:
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
}
