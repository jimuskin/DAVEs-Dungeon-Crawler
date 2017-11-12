using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject target;

    public float xOffset, yOffset, zOffset;

    private Vector3 oldCameraOffset, currentCameraPosition, targetPos;

    private float lerpTime, lerpStart;

    private bool _lerp;

    private GameObject _prevWall;

	void Update ()
    {
        setCameraPosition();
        hideBlockingWalls();
	}


    //Set the camera offset, and transition the camera whenever triggered.
    private void setCameraPosition()
    {
        targetPos = target.transform.position;

        //Point A for camera transition. Allows for player movement while camera transitions
        Vector3 oldCamera = targetPos + oldCameraOffset;

        //Point B, or camera position if camera isn't transitioning
        Vector3 cameraPosition = new Vector3(targetPos.x + xOffset, targetPos.y + yOffset, targetPos.z + zOffset);

        //Check if camera is currently transitioning, and set position accordingly.
        if (_lerp)
        {
            float prevTime = Time.time - lerpStart;

            float lerp = prevTime / lerpTime;

            currentCameraPosition = Vector3.Lerp(oldCamera, cameraPosition, lerp);

            transform.position = currentCameraPosition;
            if (lerp >= 1.0f)
            {
                _lerp = false;
            }
        }
        else
        {
            transform.position = cameraPosition;
        }

        transform.LookAt(target.transform);
    }

    //Hide any walls which are blocking the player.
    private void hideBlockingWalls()
    {
        Ray cameraRay = new Ray(target.transform.position, transform.position - target.transform.position);
        RaycastHit hit;

        int layers = LayerMask.GetMask("Wall");

        if (Physics.Raycast(cameraRay, out hit, 1000, layers))
        {
            Color colour;
 
            if (_prevWall != null)
            {
                colour = _prevWall.GetComponent<Renderer>().material.color;
                _prevWall.GetComponent<Renderer>().material.color = new Color(colour.r, colour.g, colour.b, 1.0f);
            }

            colour = hit.collider.gameObject.GetComponent<Renderer>().material.color;
            hit.collider.gameObject.GetComponent<Renderer>().material.color = new Color(colour.r, colour.g, colour.b, 0.25f);

            _prevWall = hit.collider.gameObject;
        }
        else
        {
            if (_prevWall != null)
            {
                Color colour;
                colour = _prevWall.GetComponent<Renderer>().material.color;
                _prevWall.GetComponent<Renderer>().material.color = new Color(colour.r, colour.g, colour.b, 1.0f);
            }
        }
    }

    //Create a smooth transition from switching camera offsets.
    public void transitionOffset(float x, float y, float z, float speed)
    {

        //Check if in a current lerp, and extract the offset at current state of lerp, otherwise just use defined offsets.
        if (!_lerp)
        {
            oldCameraOffset = new Vector3(xOffset, yOffset, zOffset);
        }
        else
        {
            oldCameraOffset = currentCameraPosition - targetPos;
        }

        xOffset = x;
        yOffset = y;
        zOffset = z;
        lerpTime = speed;
        lerpStart = Time.time;
        _lerp = true;
    }
}
