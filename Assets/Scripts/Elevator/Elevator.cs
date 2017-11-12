using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Elevator : MonoBehaviour
{

    private bool _elevating = false;

    public bool _goingDown;
    public bool canUse;

    public float _speed;

    public Transform liftBottom, liftTop;
    public GameObject controlStand;


    private GameObject player;
    private Animator animator;


    //Initialize the lift, and open the doors
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();


        liftTop.gameObject.GetComponent<NavMeshObstacle>().enabled = !_goingDown;
        liftBottom.gameObject.GetComponent<NavMeshObstacle>().enabled = _goingDown;

        openDoors();
    }

    void Update ()
    {
        //Continue the elevation all the way to the top.
        if (_elevating)
        {
            elevate();
        }
    }

    //Stop player movement, play animation and begin the elevator's movement
    public void operateElevators()
    {
        if (!_elevating)
        {
            if (canUse)
            {

                //TODO play lever pull animation, and face the controller
                player.transform.LookAt(controlStand.transform);

                //Disable the nav agent so the elevator can pick up the player.
                player.GetComponent<NavMeshAgent>().enabled = false;
                player.transform.GetChild(0).GetComponent<Animator>().SetBool("Elevating", true);


                closeDoors();
                elevate();

                _elevating = true;
            }
        }
    }


    //elevate 
    private void elevate()
    {
        //Moving the elevator and the player with the elevator.
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(getLift().position.x, getLift().position.y, getLift().position.z), _speed * Time.deltaTime);

        player.transform.SetParent(transform);


        if (_goingDown)
        {
            if(transform.position.y <= liftBottom.position.y)
            {
                endElevate();
            }
        }
        else
        {
            if (transform.position.y >= liftTop.position.y)
            {
                endElevate();
            }
        }
    }

    private Transform getLift()
    {
        return (_goingDown ? liftBottom : liftTop);
    }

    private void endElevate()
    {
        //Elevator has reached its destination. Stop the elevator and re-enable the nav agent.
        _goingDown = !_goingDown;
        _elevating = !_elevating;


        //Disabling respecitve colliders so that the navagent doesn't get confused. Also adding a navmeshobstacle so the player can't walk into the middle of the air.
        liftBottom.gameObject.GetComponent<BoxCollider>().enabled = !_goingDown;
        liftTop.gameObject.GetComponent<BoxCollider>().enabled = _goingDown;

        liftBottom.gameObject.GetComponent<NavMeshObstacle>().enabled = _goingDown;
        liftTop.gameObject.GetComponent<NavMeshObstacle>().enabled = !_goingDown;

        player.transform.SetParent(null);
        player.GetComponent<NavMeshAgent>().enabled = true;

        player.transform.GetChild(0).GetComponent<Animator>().SetBool("Elevating", false);
        openDoors();
    }


    //Animate the doors so they close
    void closeDoors()
    {
        if(_goingDown)
        {
            animator.SetTrigger("CloseLeft");
        }
        else
        {
            animator.SetTrigger("CloseRight");
        }
    }


    //Animate the doors so they open
    void openDoors()
    {
        if (_goingDown)
        {
            animator.SetTrigger("OpenLeft");
        }
        else
        {
            animator.SetTrigger("OpenRight");
        }
    }
}
