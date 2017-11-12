using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevationTrigger : MonoBehaviour
{
    public Elevator elevator;

    void OnTriggerEnter(Collider other)
    {
        //Allow access to elevator on elevator click.
        if(other.tag == "Player")
        {
            elevator.canUse = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Player can no longer use the elevator as they are not in range.
        if(other.tag == "Player")
        {
            elevator.canUse = false;
        }
    }
}