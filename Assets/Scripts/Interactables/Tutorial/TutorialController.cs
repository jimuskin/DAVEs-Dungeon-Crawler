using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : Interactable
{
    private TutorialControl _tutorialController;

    void Start()
    {
        _tutorialController = TutorialControl.getInstance();
    }

    public Elevator elevator;

    //Player has clicked on the elevator control panel. Elevators now in operation
    public override void onInteract()
    {
        elevator.operateElevators();

        if(_tutorialController.currentState == TutorialState.ELEVATOR)
        {
            _tutorialController.currentState = TutorialState.MOUSE;
            GUIControl.getInstance().tutorialText.text = "Nice! Now you've successfully operated the elevator. Proceed to the end to begin your major tasks.";
        }
    }
}
