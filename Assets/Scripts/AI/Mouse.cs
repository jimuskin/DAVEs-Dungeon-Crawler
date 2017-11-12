using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mouse : MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent _navAgent;

    private GameControl _gameController;

    public ParticleSystem explosion;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _navAgent = GetComponent<NavMeshAgent>();

        _gameController = GameControl.getInstance();
    }

    // TODO when the mouse is within X distance of the player chase it.
    void Update()
    {
        //When tutorial is complete, mouse dies.
        if (_gameController.currentGameState == GameState.END)
        {
            Destroy(this.gameObject);
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < 3.0f)
        {
            if (_gameController.currentGameState == GameState.GAME)
            {
                if (_navAgent.isActiveAndEnabled)
                {
                    _navAgent.Resume();
                    _navAgent.SetDestination(_player.transform.position);
                }
            }
        }
    }
    //Mouse ran into the player. Stun him for X seconds.
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (_gameController.currentGameState == GameState.GAME)
            {
                Destroy(this.gameObject);
                _player.GetComponent<PlayerControl>().stun(UnityEngine.Random.Range(1, 5));
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
