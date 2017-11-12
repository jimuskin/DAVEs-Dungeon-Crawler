using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Handles anything relevant to the player.
public class PlayerControl : MonoBehaviour
{
    private NavMeshAgent _agent;

    private GameControl _gameController;
    private GUIControl _guiController;
    private StatsControl _statsController;

    private Animator _animator;

    //Variables used for calculating player stun.
    public bool stunned { get; private set; }

    private float _prevTimeStunned, _durationStunned;

    //Used mostly for animation purposes.
    private bool _isWalking;

    //Used for finding out clicking and movement info.
    private GameObject _targetObject;
    private Vector3 _destination;

    //Not a singleton class as it can be references through the player.
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _gameController = GameControl.getInstance();
        _guiController = GUIControl.getInstance();
        _statsController = StatsControl.getInstance();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layers = LayerMask.GetMask("Ground", "Interactable");

            if (Physics.Raycast(ray, out hit, 100, layers))
            {
                //Check if the player can move (some gamestates won't allow this).
                if (_gameController.canPlayerMove)
                {
                    //Make sure the player can actually move before doing checks.
                    if (!stunned)
                    {
                        //Make sure the navAgent is enabled (it disables on things such as lifts)
                        if (_agent.isActiveAndEnabled)
                        {
                            if (hit.collider.tag == "Ground")
                            {
                                _isWalking = true;

                                _targetObject = null;
                                _destination = hit.point;

                                _agent.Resume();
                                _agent.SetDestination(_destination);
                            }

                            if (hit.collider.tag == "Interactable")
                            {
                                _isWalking = true;

                                _targetObject = hit.collider.gameObject;
                                _destination = hit.point;

                                _agent.Resume();
                                _agent.SetDestination(_destination);
                            }
                        }
                    }
                }
            }
        }


        //Check if the player has reached destination, interact with items and stop movement animation.
        //Check whether target was an object or just the ground.
        if (_targetObject != null)
        {
            if (Vector3.Distance(transform.position, _destination) <= 1.6f)
            {
                if (_agent.isActiveAndEnabled)
                {
                     _isWalking = false;
                    _agent.Stop();

                    interactWithObject(_targetObject);
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, _destination) < 0.1f)
            {
                _isWalking = false;

                _agent.Stop();
            }
        }
        if (stunned)
        {
            _durationStunned -= (Time.time - _prevTimeStunned);

            _guiController.stunnedText.text = _durationStunned.ToString("0.00");
            if (_durationStunned <= 0)
            {
                _durationStunned = 0;
                stunned = false;
            }
        }
        else
        {
            _guiController.stunnedText.text = "";
        }

        _prevTimeStunned = Time.time;
        _animator.SetBool("Walking", _isWalking);
    }

    //Call the onInteract which interactables will inherit. Some classes have it in their parent class so need to check that.
    private void interactWithObject(GameObject targetObject)
    {
        if (targetObject.GetComponent<Interactable>() != null || targetObject.transform.parent.GetComponent<Interactable>() != null)
        {
            Interactable interactable = targetObject.GetComponent<Interactable>();
            
            if(interactable == null)
            {
                interactable = targetObject.transform.parent.GetComponent<Interactable>();
            }
            interactable.onInteract();

            _targetObject = null;
        }   
    }

    public void stun(float duration)
    {
        stunned = true;
        _isWalking = false;

        _durationStunned = duration;

        if (_agent.isActiveAndEnabled)
        {
            _agent.Stop();
        }

        _statsController.addStunCount();
        _gameController.stunCount++;

        _guiController.stunnedText.text = _durationStunned.ToString("0.00");
    }

    public NavMeshAgent getNavMeshAgent()
    {
        return _agent;
    }
}
