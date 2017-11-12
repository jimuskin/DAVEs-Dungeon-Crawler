using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public float xOffset, yOffset, zOffset, speed;

    //The triggers which allow the camera to transition angles.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Camera.main.GetComponent<CameraScript>().transitionOffset(xOffset, yOffset, zOffset, speed);
        }       
    }
}
