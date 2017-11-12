using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTutorialTrigger : MonoBehaviour
{
    private GameControl _gameController;

    private void Start()
    {
        _gameController = GameControl.getInstance();
    }
    

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if (_gameController.currentGameState == GameState.TUTORIAL)
            {
                _gameController.setCurrentGameState(GameState.GAME);
            }
        }
    }
}
