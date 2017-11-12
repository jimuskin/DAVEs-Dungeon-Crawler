using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Modified Mouse class for tutorial
public class MouseTutorial : MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent _navAgent;

    private GameControl _gameController;
    private TutorialControl _tutorialController;
    private GUIControl _guiController;

    public ParticleSystem explosion;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _navAgent = GetComponent<NavMeshAgent>();

        _guiController = GUIControl.getInstance();
        _gameController = GameControl.getInstance();
        _tutorialController = TutorialControl.getInstance();
    }
    // TODO when the mouse is within X distance of the player chase it.
    void Update()
    {
        //When tutorial is complete, mouse dies.
        if (_gameController.currentGameState == GameState.GAME)
        {
            Destroy(this.gameObject);
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < 3.0f)
        {
            if (_gameController.currentGameState == GameState.TUTORIAL)
            {
                if (_tutorialController.currentState == TutorialState.MOUSE)
                {
                    if (_navAgent.isActiveAndEnabled)
                    {
                        _navAgent.Resume();
                        _navAgent.SetDestination(_player.transform.position);
                    }
                }
            }
        }
    }
    //Mouse ran into the player. Stun him for X seconds.
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (_gameController.currentGameState == GameState.TUTORIAL)
            {
                if (_tutorialController.currentState == TutorialState.MOUSE)
                {
                    Destroy(this.gameObject);
                    _player.GetComponent<PlayerControl>().stun(3);

                    _guiController.tutorialText.text = "Ouch that looked like it hurt! Careful of those evil mice, they're actually viruses which cause paralysis. Make sure to avoid them at all costs!";
                    _tutorialController.currentState = TutorialState.COMPLETE;
                }
            }
        }
    }

    void OnDestroy()
    {
        ParticleSystem explode = Instantiate(explosion);
        explode.transform.position = transform.position;

        Destroy(explode, 3f);
    }
}