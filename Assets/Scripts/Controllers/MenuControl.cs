using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuControl : MonoBehaviour
{
    //Initial camera offset when game starts
    public float posX, posY, posZ;

    private CameraScript _camera;

    private GUIControl _guiController;
    private StatsControl _statsController;

    void Start()
    {
        _guiController = GUIControl.getInstance();
        _statsController = StatsControl.getInstance();
    }


    public void startGame()
    {
        _guiController.hideUI();
        _camera = Camera.main.GetComponent<CameraScript>();

        //TODO add smooth camera transition and begin the game.
        _camera.transitionOffset(_camera.xOffset, posY, _camera.zOffset, 1);
        Invoke("offsetB", 1);
    }


    //Part 2 of the transition of the offset.
    private void offsetB()
    {
        _camera.transitionOffset(posX, posY, posZ, 2);

        Invoke("transitionIntoGame", 2);
    }

    //Start the game.
    private void transitionIntoGame()
    {
        GameControl.getInstance().setCurrentGameState(GameState.TUTORIAL);
        _statsController.addPlayCount();
    }

    //Puts the game into a special scene which will reload the game. Directly reloading the game breaks unity for some reason.
    public void reloadGame()
    {
        SceneManager.LoadScene("Test Scene");
    }


    public void showStatistics()
    {
        _guiController.statsBestTime.text = _statsController.bestTime.ToString("0.00");
        _guiController.statsTimesPlayed.text = _statsController.playCount.ToString();
        _guiController.statsTimesStunned.text = _statsController.stunnedAmount.ToString();

        _guiController.setActiveUI("STATISTICS");
        _guiController.menuStatistics.text = "Statistics";
    }

    public void hideStatistics()
    {
        _guiController.setActiveUI(GameState.MAINMENU);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
