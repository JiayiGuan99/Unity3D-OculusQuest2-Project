using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject OVRPlayerController;
    public GameObject RightHandAnchor;
    public GameObject LeftHandAnchor;
    private bool done;
    private bool done2;

    void Start(){
        done = false;
        done2 = false;
    }

    void Update()
    {
        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.9 && !done)
        {
            done = true;
            Vector3 previousLocation = OVRPlayerController.transform.position;
            RaycastHit hit;
            //Debug.Log("camera "+ OVRPlayerController.transform.position.ToString());
            if (Physics.Raycast(LeftHandAnchor.transform.position, LeftHandAnchor.transform.forward, out hit))
            {
                GameObject selection = hit.collider.gameObject;
                //Debug.Log("sfaffaf "+ hit.collider);
                if (selection.tag == "floor")
                {
                    //Debug.Log("teleport to "+hit.point.ToString());
                    OVRPlayerController.transform.position = new Vector3(hit.point.x, 2.3f, hit.point.z);
                    //Debug.Log("camera "+ OVRPlayerController.transform.position.ToString());
                    // Vector2 joystickInput = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
                    // Debug.Log("1938938      " + joystickInput );
                    // if (joystickInput.magnitude > 0.1f)
                    // {
                    //     float angle = Mathf.Atan2(joystickInput.x, joystickInput.y) * Mathf.Rad2Deg;
                    //     OVRPlayerController.transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    // }
                }
            }
            done = false;

            // if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) == 0.0)
            // {
            //     done = false;
            // }

        }
        
        if(OVRInput.Get(OVRInput.RawButton.RThumbstick) && !done2){
            done2 = true;
            Quaternion currentRotation = OVRPlayerController.transform.rotation;
            OVRPlayerController.transform.rotation = currentRotation*Quaternion.Euler(0f, 2.0f, 0f);
            done2 = false;
        }
    }
}
