using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorContoller : Interactable
{
    public Elevator elevator;

    //Player has clicked on the elevator control panel. Elevators now in operation
    public override void onInteract()
    {
        elevator.operateElevators();


        //Destroy all enemies who are intefering with the elevation and pathfinding by adding an EMP into the elevation control panel which kills all enemies inside the area

        Collider[] enemies = Physics.OverlapSphere(transform.parent.position, 4f, LayerMask.GetMask("Enemy"));

        foreach (Collider enemy in enemies)
        {
            if (enemy.tag == "Enemy")
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}
