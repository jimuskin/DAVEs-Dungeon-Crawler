using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : Interactable
{

    private GameControl _gameController;
    private GUIControl _guiController;

    private String prevText;

    void Start()
    {
        _gameController = GameControl.getInstance();
        _guiController = GUIControl.getInstance();
    }

    //TODO add usable EMP to player inventory, and tell the player he picked up an EMP
    public override void onInteract()
    {
        _gameController.empCount++;
        _guiController.temporaryText("You picked up an EMP grenade!Right click to destroy all evil mice in a short radius!", 4);
        Destroy(this.gameObject);
    }
}
