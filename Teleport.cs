using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject teleportIndicator; // prefab for teleport indicator
    public Transform cameraRig; // object containing camera and controllers
    public float teleportRange = 10f; // max distance for teleportation
    public OVRInput.Controller teleportController; // controller to use for teleportation

    private GameObject indicator; // instance of teleport indicator
    private bool isTeleporting = false; // flag for teleportation
    private Vector3 teleportLocation; // location to teleport to

    // Update is called once per frame
    void Update()
    {
        // Get input from right-hand controller
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, teleportController);

        if (joystickInput.magnitude > 0.1f) {
            // Update camera orientation based on joystick input
            float angle = Mathf.Atan2(joystickInput.x, joystickInput.y) * Mathf.Rad2Deg;
            cameraRig.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Check for input to start teleportation
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, teleportController)) {
            // Create teleport indicator
            indicator = Instantiate(teleportIndicator, transform.position, Quaternion.identity);

            // Set flag for teleportation
            isTeleporting = true;
        }

        // Check for input to end teleportation
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, teleportController) && isTeleporting) {
            // Teleport to selected location
            cameraRig.position = teleportLocation;

            // Destroy teleport indicator
            Destroy(indicator);

            // Reset flag for teleportation
            isTeleporting = false;
        }

        // Update teleport indicator position if teleportation is active
        if (isTeleporting) {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);

            // Cast a ray from the controller and update teleport indicator position
            if (Physics.Raycast(ray, out hit, teleportRange)) {
                teleportLocation = hit.point;
                indicator.transform.position = hit.point;
            } else {
                teleportLocation = transform.position + transform.forward * teleportRange;
                indicator.transform.position = teleportLocation;
            }
        }
    }
}