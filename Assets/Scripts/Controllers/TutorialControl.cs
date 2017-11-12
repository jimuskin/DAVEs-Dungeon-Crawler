using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControl : MonoBehaviour
{
    private static TutorialControl instance;

    private GUIControl _guiController;

    public TutorialState currentState;

    public TutorialControl()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        _guiController = GUIControl.getInstance();
    }

    //Set up the tutorial, show the player, and break the machine.
    public void setupTutorial()
    {
        _guiController.tutorialText.text = "Welcome back DAVE! We need your help right now, mind clicking and holding your mouse button on that machine over there?";
        currentState = TutorialState.MACHINE;
    }

    public static TutorialControl getInstance()
    {
        return instance;
    }
}

public enum TutorialState
{
    MACHINE, ELEVATOR, MOUSE, COMPLETE
}
