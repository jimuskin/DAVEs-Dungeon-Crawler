using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIControl : MonoBehaviour
{
    private static GUIControl instance;

    public GameObject UI;

    //The UI associated to the tutorial
    public Text tutorialText;
    public Slider tutorialSlider; 

    //The UI associated to the game.
    public Slider gameSlider;
    public Text gameTimer;
    public Text gameRemainMachines;
    public Text gameMachineText;

    //The UI associated to the end screen.
    public Text endCurrentTime;
    public Text endBestTime;
    public Text endStunnedTimes;

    //The UI associated to the main menu
    public Text menuStatistics;

    //The UI associated to the statistics menu
    public Text statsTimesPlayed;
    public Text statsBestTime;
    public Text statsTimesStunned;

    public Text stunnedText;

    private string _prevText;

    public GUIControl()
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

    //Hides all GUI elements
    public void hideUI()
    {
        foreach(Transform child in UI.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    //Sets the active GUI
    public void setActiveUI(GameState state)
    {
        foreach(Transform child in UI.transform)
        {
            if(child.gameObject.name == state.ToString())
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        stunnedText.gameObject.SetActive(true);
    }


    //Sets the active GUI by name
    public void setActiveUI(string name)
    {
        foreach (Transform child in UI.transform)
        {
            if (child.gameObject.name == name)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        stunnedText.gameObject.SetActive(true);
    }


    public void temporaryText(string text, float duration)
    {

        _prevText = gameMachineText.text;

        gameMachineText.text = text;

        Invoke("resetText", duration);
    }

    private void resetText()
    {
        gameMachineText.text = _prevText;
    }

    public static GUIControl getInstance()
    {
        return instance;
    }
}