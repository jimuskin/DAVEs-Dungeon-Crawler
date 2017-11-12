using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles anything game related.
public class GameControl : MonoBehaviour
{

    private static GameControl instance;

    public GameState currentGameState { get; private set; }
    public bool canPlayerMove { get; private set; }

    public int stunCount;

    public int empCount;

    private MachineControl _machineController;
    private GUIControl _guiController;
    private TutorialControl _tutorialController;

    //Time to tell how long the player has been playing for (starts when gamestate set to game).
    private float _prevTime;
    private float currentTime;
    private bool _calculatingTime;

    public ParticleSystem empParticle;
    private ParticleSystem _currentEMP;

    //These are the offsets used for the camera when the game is complete.
    public float endGameX, endGameY, endGameZ;

    public GameControl()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    void Start()
    {
        _machineController = MachineControl.getInstance();
        _guiController = GUIControl.getInstance();
        _tutorialController = TutorialControl.getInstance();

        setCurrentGameState(GameState.MAINMENU);
    }


    void Update()
    {
        if (_calculatingTime)
        {
            currentTime += (Time.time - _prevTime);

            _guiController.gameTimer.text = currentTime.ToString("0.000");
        }

        //On right click, destroy all enemies in short radius if player has EMP grenades.
        if(Input.GetButtonDown("Fire2"))
        {
            if (empCount > 0)
            {
                _currentEMP = Instantiate(empParticle);
                _currentEMP.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;

                Collider[] enemies = Physics.OverlapSphere(GameObject.FindGameObjectWithTag("Player").transform.position, 2f, LayerMask.GetMask("Enemy"));

                foreach (Collider enemy in enemies)
                {
                    if (enemy.tag == "Enemy")
                    {
                        Destroy(enemy.gameObject);
                    }
                }
                empCount--;
            }
        }

        _prevTime = Time.time;
    }

    public void setCurrentGameState(GameState state)
    {
        currentGameState = state;
        _guiController.setActiveUI(state);

        //TODO make game do stuff depending on game state.

        //Game will perform actions based on the state it has switched into.
        switch (state)
        {
            case GameState.MAINMENU:
                canPlayerMove = false;
                break;

            case GameState.TUTORIAL:
                canPlayerMove = true;
                _tutorialController.setupTutorial();
                break;


            case GameState.GAME:
                _machineController.breakRandomMachine();
                _calculatingTime = true;
                canPlayerMove = true;
                break;

            case GameState.END:

                calculateStats();

                GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Animator>().SetBool("Victory", true);
                _guiController.setActiveUI(GameState.END);
                break;
        }

    }

    private void calculateStats()
    {
        float bestTime = StatsControl.getInstance().bestTime;

        _guiController.endCurrentTime.text = currentTime.ToString("0.000");

        if (bestTime > 0.0f)
        {
            //Highscore has been reached
            if (currentTime < bestTime)
            {
                _guiController.endBestTime.text = currentTime.ToString("0.000");
                StatsControl.getInstance().setBestTime(currentTime);
            }
            else
            {
                _guiController.endBestTime.text = bestTime.ToString("0.000");
            }
        }
        else
        {
            _guiController.endBestTime.text = currentTime.ToString("0.000");
            StatsControl.getInstance().setBestTime(currentTime);
        }

        _guiController.endStunnedTimes.text = stunCount.ToString();
    }


    //This creates the smooth ending using the camera transforms and by invoking methods. Parts A to C are different transitions.

    public void endGame()
    {
        canPlayerMove = false;
        _calculatingTime = false;
        _guiController.hideUI();
        transitionEnding_a();
    }

    private void transitionEnding_a()
    {
        CameraScript camera = Camera.main.GetComponent<CameraScript>();
        camera.transitionOffset(camera.xOffset, endGameY + 2, endGameZ, 1.5f);

        Invoke("transitionEnding_b", 1.5f);
    }

    private void transitionEnding_b()
    {
        CameraScript camera = Camera.main.GetComponent<CameraScript>();
        camera.transitionOffset(endGameX, endGameY, endGameZ, 1.5f);
        
         GameObject.FindGameObjectWithTag("Player").transform.rotation = Quaternion.identity;

        Invoke("transitionEnding_c", 1.5f);
    }

    private void transitionEnding_c()
    {
        setCurrentGameState(GameState.END);
    }






    public static GameControl getInstance()
    {
        return instance;
    }

}




public enum GameState
{
    MAINMENU, TUTORIAL, GAME, END
}