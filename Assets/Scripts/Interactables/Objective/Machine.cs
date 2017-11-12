using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Machine : Interactable
{
    private MachineState _state;

    //Used for player fixing
    private float _currentTime = 0;
    private float _prevTime;
    private bool _isFixing;

    private PlayerControl _playerControl;
    private GameObject _player;

    private ParticleSystem _currentSparks;
    
    private GameControl _gameController;
    private GUIControl _guiController;
    private MachineControl _machineController;

    public String brokenText;

    //Need to prioritise the initial setup before any other script tries to access it
    void Awake()
    {
        setState(MachineState.FIXED);

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerControl = _player.GetComponent<PlayerControl>();

        _gameController = GameControl.getInstance();
        _guiController = GUIControl.getInstance();
        _machineController = MachineControl.getInstance();
    }

    //Begin the interaction with the machine, and state that the machine is now being fixed.
    public override void onInteract()
    {
        if(_machineController.currentMachine != null && _machineController.currentMachine == this)
        {
            if (_gameController.currentGameState == GameState.GAME)
            {
                if (!_playerControl.stunned)
                {
                    //TODO start the fixing process. Play particle effects for x seconds
                    _currentTime = 0;
                    _isFixing = true;
                    _currentSparks = Instantiate(_machineController.sparkParticles);
                    _currentSparks.transform.position = transform.position + new Vector3(0, 1, 0);
                }
            }
        }
    }

    //Check if the player is holding down the mouse button, and continue the fixing process if so. Also check to see if the player has completed his fixing on the machine and decrements remaining machines if so.
    private void Update()
    {
        if (_machineController.currentMachine != null && _machineController.currentMachine == this)
        {
            if (Input.GetButton("Fire1"))
            {
                if(_gameController.currentGameState == GameState.GAME)
                {
                    if (_isFixing)
                    {
                        if (!_playerControl.stunned)
                        {
                            _currentTime += (Time.time - _prevTime);
                            _player.transform.GetChild(0).GetComponent<Animator>().SetBool("Fixing", true);
                            _player.transform.LookAt(this.transform);

                            if (_currentTime >= ((int)_state) * 2)
                            {
                                setState(MachineState.FIXED);

                                //Decrement remaining machines. Checks if game has ended also.
                                if (!_machineController.decrementMachines())
                                {
                                    _machineController.setCurrentMachine(null);

                                    _machineController.breakRandomMachine(this);
                                }

                                resetTimer();
                            }
                        }
                        else
                        {
                            resetTimer();
                        }
                    }
                }
            }
            else
            {
                resetTimer();
            }
            _guiController.gameSlider.value = _currentTime;
            _guiController.gameSlider.maxValue = (int)_state * 2;
        }

        _prevTime = Time.time;
    }


    //Resets the timer values
    private void resetTimer()
    {
        _currentTime = 0;

        _isFixing = false;

        if (_currentSparks != null)
        {
            Destroy(_currentSparks.gameObject);
            _currentSparks = null;
        }

        _player.transform.GetChild(0).GetComponent<Animator>().SetBool("Fixing", false);
    }


    //Sets the machine's state, then finds the correct gameobject for it.
    public void setState(MachineState state)
    {
        _state = state;

        foreach (Transform child in this.transform)
        {
            if (child.gameObject.name == state.ToString())
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public static int getStateSize()
    {
        return Enum.GetValues(typeof(MachineState)).Cast<int>().Max();
    }
}

public enum MachineState
{
    DESTROYED = 3, BROKEN = 2, FIXED = 1
}