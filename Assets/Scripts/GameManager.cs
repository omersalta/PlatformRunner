using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private State currentState;
    private InputState IS;
    
    enum State {
        GET_INPUT,
        GAME_RUNNING,
        GAME_WON,
        GAME_OVER,
        RESTART,
        NEXT_GAME,
    }
    
    void Start() {
        IS = FindObjectOfType<InputState>();
        currentState = State.GET_INPUT;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState) {
            case State.GET_INPUT:
                Debug.Log("game manager GET_INPUT");
                if (IS.anyTap) {
                    UnlockAllStartLocks();
                    currentState = State.GAME_RUNNING;
                }
                break;
            case State.GAME_RUNNING:
                //idle loop
                
                break;
            case State.GAME_WON:
                
                break;
            case State.GAME_OVER:
                Debug.Log("game manager gameover");
                lockAllStartLocks();
                break;
            case State.RESTART:
                currentState = State.GET_INPUT;
                break;
            case State.NEXT_GAME:
                
                break;
        }
    }
    
    private void UnlockAllStartLocks() {
        Bot.startLock = false;
        FindObjectOfType<CharacterControl>().Unlock();
        FindObjectOfType<Spawner>().ActiveSpawner();
    }
    
    private void lockAllStartLocks() {
        Bot.startLock = true;
        FindObjectOfType<CharacterControl>().Lock();
        FindObjectOfType<Spawner>().active = false;
    }
    
    
    public void PlayButton() {
        
    }

    public void GameOver() {
        currentState = State.GAME_OVER;
    }
}
