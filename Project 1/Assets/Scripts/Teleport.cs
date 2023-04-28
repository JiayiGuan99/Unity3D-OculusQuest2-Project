using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject OVRPlayerController;
    public GameObject RightHandAnchor;
    public GameObject LeftHandAnchor;
    private bool done = false;

    void Update()
    {
        while (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.9 && !done)
        {
            done = true;
            RaycastHit hit;
            Debug.Log("camera "+ OVRPlayerController.transform.position.ToString());
            if (Physics.Raycast(LeftHandAnchor.transform.position, LeftHandAnchor.transform.forward, out hit))
            {
                GameObject selection = hit.collider.gameObject;
                Debug.Log("sfaffaf "+ hit.collider);
                if (selection.tag == "floor")
                {
                    Debug.Log("teleport to "+hit.point.ToString());
                    OVRPlayerController.transform.position = new Vector3(hit.point.x, 2.3f, hit.point.z);
                    Debug.Log("camera "+ OVRPlayerController.transform.position.ToString());
                    Vector2 joystickInput = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
                    if (joystickInput.magnitude > 0.1f)
                    {
                        float angle = Mathf.Atan2(joystickInput.x, joystickInput.y) * Mathf.Rad2Deg;
                        OVRPlayerController.transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    }
                }

                
            }
        }
        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) == 0.0)
        {
            done = false;
        }
    }
}
