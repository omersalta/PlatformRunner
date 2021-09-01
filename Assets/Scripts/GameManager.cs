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
    public GameObject PANEL;
    
    enum State {
        INITILIZE,
        GET_INPUT,
        GAME_RUNNING,
        GAME_OVER,
        GAME_WON,
        RESTART,
        NEXT_GAME,
    }
    
    void Start() {
        IS = FindObjectOfType<InputState>();
        currentState = State.INITILIZE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState) {
            
            case State.INITILIZE:
                lockAllStartLocks();
                FindObjectOfType<FallowingCamera>().FallowTarget();
                currentState = State.GET_INPUT;
                break;
            case State.GET_INPUT:
                //Debug.Log("game manager GET_INPUT");
                if (IS.anyTap) {
                    UnlockAllStartLocks();
                    PlayGame();
                    currentState = State.GAME_RUNNING;
                }
                break;
            case State.GAME_RUNNING:
                //idle loop
                
                break;
            case State.GAME_WON:
                //Debug.Log("game manager GAME WON!!!!");
                break;
            case State.GAME_OVER:
                //Debug.Log("game manager gameover");
                break;
            case State.RESTART:
                currentState = State.GET_INPUT;
                break;
            case State.NEXT_GAME:
                break;
        }
    }
    
    private void UnlockAllStartLocks() {
        FindObjectOfType<CharacterControl>().Unlock();
        FindObjectOfType<Spawner>().ActiveSpawner();
        FindObjectOfType<FallowingCamera>().FallowTarget();
    }
    
    private void lockAllStartLocks() {
        FindObjectOfType<CharacterControl>().Lock();
        FindObjectOfType<Spawner>().active = false;
        FindObjectOfType<FallowingCamera>().UnFallowTarget();
    }
    
    
    public void PlayGame() {
        FindObjectOfType<PannelManager>().GameStart();
    }

    public void GameOver() {
        lockAllStartLocks();
        FindObjectOfType<PannelManager>().Gameover();
        currentState = State.GAME_OVER;
    }
    
    public void GameWon() {
        lockAllStartLocks();
        FindObjectOfType<PannelManager>().GameWon();
        currentState = State.GAME_WON;
    }
}
