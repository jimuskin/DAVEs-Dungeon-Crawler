using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Pretty much the same as the normal machine, except modified slightly for tutorial purposes.
public class TutorialMachine : Interactable
{
    private GameControl _gameController;
    private GUIControl _guiController;
    private TutorialControl _tutorialController;

    private GameObject _player;

    private float _currentTime = 0;
    private float _prevTime;
    private bool _isFixing;
    private bool _isBroken;


    public ParticleSystem sparkParticles;
    private ParticleSystem _currentSparks;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _gameController = GameControl.getInstance();
        _guiController = GUIControl.getInstance();
        _tutorialController = TutorialControl.getInstance();

        setState(false);
    }


    public override void onInteract()
    {
        if (_gameController.currentGameState == GameState.TUTORIAL)
        {
            if (_tutorialController.currentState == TutorialState.MACHINE)
            {
                if (_isBroken)
                {
                    //TODO start the fixing process. Play particle effects for x seconds
                    _currentTime = 0;
                    _isFixing = true;
                    _currentSparks = Instantiate(sparkParticles);
                    _currentSparks.transform.position = transform.position + new Vector3(0, 1, 0);
                }
            }
        }
    }
	
	void Update ()
    {
        if (_gameController.currentGameState == GameState.TUTORIAL)
        {
            if (_tutorialController.currentState == TutorialState.MACHINE)
            {
                if (Input.GetButton("Fire1"))
                {
                    if (_isBroken)
                    {
                        if (_isFixing)
                        {
                            _currentTime += (Time.time - _prevTime);
                            _player.transform.GetChild(0).GetComponent<Animator>().SetBool("Fixing", true);
                            _player.transform.LookAt(this.transform);

                            //Tutorial machine takes 4 seconds to fix.
                            if (_currentTime >= 4)
                            {
                                fixMachine();

                                resetTimer();
                            }
                        }
                    }
                }
                else
                {
                    resetTimer();
                }

                _guiController.tutorialSlider.value = _currentTime;
                _guiController.tutorialSlider.maxValue = 4;
            }
        }
        _prevTime = Time.time;
    }

    private void resetTimer()
    {
        _currentTime = 0;

        _isFixing = false;

        if(_currentSparks != null)
        {
            Destroy(_currentSparks.gameObject);
            _currentSparks = null;
        }

        _player.transform.GetChild(0).GetComponent<Animator>().SetBool("Fixing", false);
    }

    public void breakMachine()
    {
        _isBroken = true;
        
    }

    public void fixMachine()
    {
        setState(true);
        _guiController.tutorialText.text = "You have successfully fixed your first machine! Proceed through the corridor until you approach the elevator, and press the elevator's control panel.";
        _tutorialController.currentState = TutorialState.ELEVATOR;
    }


    //Sets the machine's state, then finds the correct gameobject for it.
    public void setState(bool state)
    {
        _isBroken = !state;

        foreach (Transform child in this.transform)
        {
            string fixedMachine = (_isBroken ? "BROKEN" : "FIXED");

            if (child.gameObject.name == fixedMachine)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
