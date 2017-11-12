using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles anything relevant to management of machines.
public class MachineControl : MonoBehaviour
{
    private static MachineControl instance;

    public Machine currentMachine { get; private set; }

    public Machine[] machines;

    public int remainingMachines;

    public ParticleSystem sparkParticles;

    private GUIControl _guiController;
    private GameControl _gameController;

    public MachineControl()
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

    private void Start()
    {
        _guiController = GUIControl.getInstance();
        _gameController = GameControl.getInstance();
    }

    private void Update()
    {
        _guiController.gameRemainMachines.text = remainingMachines.ToString();
    }

    //Break a random machine.
    public void breakRandomMachine()
    {
        int randomInt = UnityEngine.Random.Range(0, machines.Length);

        Machine machine = machines[randomInt];

        setCurrentMachine(machine);
        machine.setState(getRandomMachineState());

        _guiController.gameMachineText.text = machine.brokenText;
    }


    //Break a random machine. Ignores the machine in arguments and doesn't allow it to break.
    public void breakRandomMachine(Machine mach)
    {
        int randomInt = UnityEngine.Random.Range(0, machines.Length);

        Machine machine = machines[randomInt];

        //Make sure the machine doesn't match the one passed in args.
        while(machine == mach)
        {
            randomInt = UnityEngine.Random.Range(0, machines.Length);
            machine = machines[randomInt];
        }

        setCurrentMachine(machine);
        machine.setState(getRandomMachineState());

        _guiController.gameMachineText.text = machine.brokenText;
    }

    //Returns true if the remaining machines has hit 0, thus ending the game/
    public bool decrementMachines()
    {
        remainingMachines--;

        if(remainingMachines <= 0)
        {
            //Game has ended.
            _gameController.endGame();
            return true;
        }

        return false;
    }


    //Get a random machine state, out of the broken ones.
    private MachineState getRandomMachineState()
    {
        //FIXED is state number one, and we don't want to break a machine to the point where it fixes itself, so start from 2.
        int randomInt = UnityEngine.Random.Range(1, Machine.getStateSize()) + 1;
        return (MachineState)randomInt;
    }


    public static MachineControl getInstance()
    {
        return instance;
    }

    public void setCurrentMachine(Machine machine)
    {
        currentMachine = machine;
    }
}
