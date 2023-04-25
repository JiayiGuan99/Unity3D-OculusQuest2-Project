using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Dropdown menu;
    public GameObject TV;
    public GameObject cabinet;
    public GameObject chair;
    public GameObject desk;
    public GameObject locker;
    public GameObject room;
    public GameObject whiteboard;

    private GameObject currentObject;
    private bool placingObject = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Update()
    {
        // if right controller's index trigger is pressed, and object is not placed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && !placingObject)
        {
            SpawnObject();
        }
        else if (placingObject)
        {
            MoveObject();
        }
    }

    private void SpawnObject()
    {
        switch (menu.value)
        {
            case 0:
                currentObject = Instantiate(TV);
                break;
            case 1:
                currentObject = Instantiate(cabinet);
                break;
            case 2:
                currentObject = Instantiate(chair);
                break;
            case 3:
                currentObject = Instantiate(desk);
                break;
            case 4:
                currentObject = Instantiate(locker);
                break;
            case 5:
                currentObject = Instantiate(room);
                break;
            case 6:
                currentObject = Instantiate(whiteboard);
                break;
            default:
                break;
        }

        if (currentObject != null)
        {
            placingObject = true;
            currentObject.GetComponent<Rigidbody>().useGravity = false;
            currentObject.GetComponent<Rigidbody>().isKinematic = true;
            initialPosition = transform.position;
            initialRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        }
    }

    private void MoveObject()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = initialPosition.y;
        currentObject.transform.position = newPosition;
        currentObject.transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Quaternion.Inverse(initialRotation);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            placingObject = false;
            currentObject.GetComponent<Rigidbody>().useGravity = true;
            currentObject.GetComponent<Rigidbody>().isKinematic = false;
            currentObject = null;
        }
    }
}
