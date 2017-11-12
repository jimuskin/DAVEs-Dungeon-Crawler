using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{

    //For some reason, the game breaks if I reload the scene. Need to move into an external scene and transition from there.
	void Awake ()
    {
        SceneManager.LoadScene("Game Scene");
	}
}
